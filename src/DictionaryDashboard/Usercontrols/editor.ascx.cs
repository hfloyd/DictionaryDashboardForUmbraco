using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.language;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace DictionaryDashboard.usercontrols
{
    public partial class editor : System.Web.UI.UserControl
    {
        private UmbracoDatabase _db = ApplicationContext.Current.DatabaseContext.Database;
        #region Event Wireup

        override protected void OnInit(EventArgs e)
        {
            ((umbraco.UmbracoDefault)this.Page).ValidateRequest = false;
            
            InitializeComponent();
            base.OnInit(e);
        }
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
            this.selItems.SelectedIndexChanged += new EventHandler(selItems_SelectedIndexChanged);
            this.btnSave.Click += new System.EventHandler(btnSave_Click);
            this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
        }

        #endregion

        #region Local Properties/Variables

        protected Language language
        {
            get { return Page.GetDataItem() as Language; }
        }

        protected List<Language> languages = new List<Language>();

        protected List<Dictionary.DictionaryItem> dictItems = new List<Dictionary.DictionaryItem>();

        #endregion

        #region Form Events

        protected void Page_Load(object sender, EventArgs e)
        {
            this.litStatus.Text = "";
            this.languages.AddRange(Language.GetAllAsList());
            //Loop through all levels of dictionary items, adding each to the list
            foreach (var root in Dictionary.getTopMostItems)
            {
                AddToList(root, ref this.dictItems);

                if (!Page.IsPostBack)
                {
                    this.selItems.DataTextField = "key";
                    this.selItems.DataValueField = "key";
                    this.selItems.DataSource = this.dictItems.OrderBy(n => n.key); //Alphabetize full list
                    this.selItems.DataBind();
                    this.selItems.Items.Insert(0, new ListItem("Choose...", ""));
                }
            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            if (Dictionary.DictionaryItem.hasKey(this.selItems.SelectedValue))
            {
                var item = new Dictionary.DictionaryItem(this.selItems.SelectedValue);
                foreach (var key in Request.Form.AllKeys)
                {
                    if (key.StartsWith(this.ClientID + "-translation-"))
                    {
                        var languageId = Convert.ToInt32(key.Split('-').Last());
                        item.setValue(languageId, Request.Form[key].Trim());
                    }
                }
            }
            this.litStatus.Text = "<div class=\"success\"><h3>Success</h3><p>The \"" + this.selItems.SelectedValue + "\" Dictionary item was updated.</p></div>";
            this.selItems.SelectedValue = "";
            this.phEdit.Visible = false;
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.selItems.SelectedValue = "";
            this.phEdit.Visible = false;
            txtTerm.Text = string.Empty;
        }

        protected void selItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTerm.Text = string.Empty;
            ShowDictionaryItem(this.selItems.SelectedValue);
        }

        #endregion

        #region Internal Methods

        protected string GetValue(int languageId)
        {
            if (Dictionary.DictionaryItem.hasKey(this.selItems.SelectedValue))
            {
                var item = new Dictionary.DictionaryItem(this.selItems.SelectedValue);
                return item.Value(languageId);
            }
            return "";
        }

        private void AddToList(Dictionary.DictionaryItem parent, ref List<Dictionary.DictionaryItem> list)
        {
            list.Add(parent);

            if (parent.hasChildren)
            {
                foreach (var item in parent.Children)
                {
                    AddToList(item, ref list);
                }
            }
        }

        #endregion

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            this.phEdit.Visible = false;
            this.selItems.SelectedValue = "";
            if (string.IsNullOrWhiteSpace(txtTerm.Text))
            {
                return;
            }
            //Search for item in cmsLanguageText table
            var res = this._db.Fetch<cmsLanguageText>(string.Format("SELECT * FROM cmsLanguageText WHERE value like LOWER(N'%{0}%')", txtTerm.Text.ToLower())).ToList();
            if (res.Count() > 1)
            {
                this.phMultipleSearch.Visible = true;
                this.repSearch.DataSource = res;
                this.repSearch.DataBind();
            }
            else if (res.Count() == 1)
            {
                var dicItem = GetDictionaryItemById(res.FirstOrDefault().UniqueId);
                ShowDictionaryItem(dicItem.key);
            }
        }
        public void clickbutton(Object sender, CommandEventArgs e)
        {
            string guid = ((LinkButton)sender).Attributes["RowID"].ToString().Trim();
            var dicItem = GetDictionaryItemById(new Guid(guid));
            ShowDictionaryItem(dicItem.key);
            this.phMultipleSearch.Visible = false;
        }
        /// <summary>
        /// Get dictionary item by guid id and PetaPoco
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        private cmsDictionary GetDictionaryItemById(Guid uniqueId)
        {
            return this._db.SingleOrDefault<cmsDictionary>(string.Format("SELECT * FROM cmsDictionary WHERE id = '{0}'", uniqueId));
        }
        /// <summary>
        /// If key is available then show the dic item 
        /// </summary>
        /// <param name="key"></param>
        private void ShowDictionaryItem(string key)
        {
            if (Dictionary.DictionaryItem.hasKey(key))
            {
                this.selItems.ClearSelection();
                this.selItems.Items.FindByValue(key).Selected = true;
                this.phEdit.Visible = true;
                this.repTranslations.DataSource = languages;
                this.repTranslations.DataBind();
            }
            else
            {
                this.phEdit.Visible = false;
            }
        }
    }


    public class cmsLanguageText
    {
        public int pk { get; set; }
        public int languageId { get; set; }
        public Guid UniqueId { get; set; }
        public string value { get; set; }
    }
    public class cmsDictionary
    {
        public int pk { get; set; }
        public Guid id { get; set; }
        public Guid parent { get; set; }
        public string key { get; set; }
    }
}
