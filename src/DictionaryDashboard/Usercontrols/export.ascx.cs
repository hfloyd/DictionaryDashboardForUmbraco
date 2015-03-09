using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.language;

namespace Futuristic.Umbraco.Packages.DictionaryDashboard.usercontrols
{
	public partial class export : System.Web.UI.UserControl
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
			this.btnExport.Click += new EventHandler(btnExport_Click);
		}
		#endregion

		protected List<Language> allLanguages = new List<Language>();
		protected List<Language> selectedLanguages = new List<Language>();

		protected void Page_Load(object sender, EventArgs e)
		{
			allLanguages.AddRange(Language.GetAllAsList());

			if (!Page.IsPostBack)
			{
				chkListLanguages.DataTextField = "FriendlyName";
				chkListLanguages.DataValueField = "id";
				chkListLanguages.DataSource = allLanguages;
				chkListLanguages.DataBind();

				foreach (ListItem item in chkListLanguages.Items)
					item.Selected = true;
			}

			foreach(ListItem item in chkListLanguages.Items)
				if(item.Selected)
					selectedLanguages.Add((from l in allLanguages where l.id == Convert.ToInt32(item.Value) select l).Single());
		}

		protected void btnExport_Click(object sender, EventArgs e)
		{
			var list = new List<Dictionary.DictionaryItem>();

			foreach (var root in Dictionary.getTopMostItems)
				AddToList(root, ref list);

			var xmlExport = new XElement("Items",
				  from i in list
				  orderby i.key
				  select new XElement("Item",
						 new XAttribute("key", i.key),
						 new XAttribute("parentKey", !i.IsTopMostItem() ? i.Parent.key : ""),
						 from l in selectedLanguages
						 select new XElement(l.CultureAlias, new XCData(i.Value(l.id)))));

			Response.ContentType = "text/xml";
			Response.AppendHeader("Content-Disposition", "attachment;filename=dictionary.xml");
			xmlExport.Save(Response.OutputStream);
			Response.End();

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