using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.language;

namespace Futuristic.Umbraco.Packages.DictionaryDashboard.usercontrols
{
	public partial class editor : System.Web.UI.UserControl
	{
		#region Event wireup
		override protected void OnInit(EventArgs e)
		{
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

		protected Language language { get { return Page.GetDataItem() as Language; } }

		protected List<Language> languages = new List<Language>();
		protected List<Dictionary.DictionaryItem> list = new List<Dictionary.DictionaryItem>();

		protected void Page_Load(object sender, EventArgs e)
		{
			litStatus.Text = "";

			languages.AddRange(Language.GetAllAsList());

			foreach (var root in Dictionary.getTopMostItems)
				AddToList(root, ref list);

			if (!Page.IsPostBack)
			{
				selItems.DataTextField = "key";
				selItems.DataValueField = "key";
				selItems.DataSource = list;
				selItems.DataBind();
				selItems.Items.Insert(0, new ListItem("Choose...", ""));
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			if (Dictionary.DictionaryItem.hasKey(selItems.SelectedValue))
			{
				var item = new Dictionary.DictionaryItem(selItems.SelectedValue);

				foreach (var key in Request.Form.AllKeys)
				{
					if (key.StartsWith(this.ClientID + "-translation-"))
					{
						var languageId = Convert.ToInt32(key.Split('-').Last());
						item.setValue(languageId, Request.Form[key].Trim());
					}
				}
			}

			litStatus.Text = "<div class=\"success\"><h3>Success</h3><p>The \"" + selItems.SelectedItem.Text + "\" Dictionary item was updated.</p></div>";

			selItems.SelectedValue = "";
			phEdit.Visible = false;
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			selItems.SelectedValue = "";
			phEdit.Visible = false;
		}

		protected void selItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Dictionary.DictionaryItem.hasKey(selItems.SelectedValue))
			{
				phEdit.Visible = true;
				repTranslations.DataSource = languages;
				repTranslations.DataBind();
			}
			else
				phEdit.Visible = false;
		}

		protected string GetValue(int languageId)
		{
			if (Dictionary.DictionaryItem.hasKey(selItems.SelectedValue))
			{
				var item = new Dictionary.DictionaryItem(selItems.SelectedValue);
				return item.Value(languageId);
			}
			return "";
		}

		private void AddToList(Dictionary.DictionaryItem parent, ref List<Dictionary.DictionaryItem> list)
		{
			list.Add(parent);

			if (parent.hasChildren)
				foreach (var item in parent.Children)
					AddToList(item, ref list);
		}
	}
}