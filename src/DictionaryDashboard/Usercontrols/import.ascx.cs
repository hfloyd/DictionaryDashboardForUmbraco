using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.language;

namespace Futuristic.Umbraco.Packages.DictionaryDashboard.usercontrols
{
	public partial class import : System.Web.UI.UserControl
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
			this.btnImport.Click += new EventHandler(btnImport_Click);
			this.btnValidate.Click += new EventHandler(btnValidate_Click);
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

			foreach (ListItem item in chkListLanguages.Items)
				if (item.Selected)
					selectedLanguages.Add((from l in allLanguages where l.id == Convert.ToInt32(item.Value) select l).Single());
		}

		protected void btnValidate_Click(object sender, EventArgs e)
		{
			XDocument doc = GetXmlDocument();

			if (doc != null)
			{
				if (Import(doc, false))
					litStatus.Text = "<div class=\"success\"><h3>Success</h3><p>Your XML is valid.</p></div>";
				else
					litStatus.Text = "<div class=\"error\"><h3>Error!</h3><p>Your XML appears to be invalid. If you try to import this XML file, the Import routine will throw an exception.</p></div>";
			}
		}

		protected void btnImport_Click(object sender, EventArgs e)
		{
			XDocument doc = GetXmlDocument();

			if (doc != null)
			{
				if (Import(doc, true))
					litStatus.Text = "<div class=\"success\"><h3>Success</h3><p>Your dictionary was successfully updated.</p></div>";
				else
					litStatus.Text = "<div class=\"error\"><h3>Error!</h3><p>An exception was thrown while trying to import your XML file.</p></div>";
			}
		}

		protected XDocument GetXmlDocument()
		{
			if (fileUploadXml.HasFile)
			{
				XDocument doc = null;

				try
				{
					doc = XDocument.Load(XmlReader.Create(fileUploadXml.PostedFile.InputStream));
				}
				catch (Exception)
				{
					litStatus.Text = "<div class=\"error\"><h3>Error!</h3><p>There was an error parsing the uploaded file.</p></div>";
					return null;
				}

				return doc;
			}
			else
			{
				litStatus.Text = "<div class=\"error\"><h3>Error!</h3><p>Please select a file.</p></div>";
				return null;
			}
		}

		protected bool Import(XDocument doc, bool commit)
		{

			try
			{
				foreach (XElement xmlItem in doc.Elements("Items").Elements("Item"))
				{
					// Create
					if (!Dictionary.DictionaryItem.hasKey(xmlItem.Attribute("key").Value))
					{
						if (xmlItem.Attribute("parentKey") != null && Dictionary.DictionaryItem.hasKey(xmlItem.Attribute("parentKey").Value))
						{
							if(commit)
								Dictionary.DictionaryItem.addKey(xmlItem.Attribute("key").Value, "", xmlItem.Attribute("parentKey").Value);
						}
						else
						{
							if (commit)
								Dictionary.DictionaryItem.addKey(xmlItem.Attribute("key").Value, "");
						}
					}

					// Update
					if (Dictionary.DictionaryItem.hasKey(xmlItem.Attribute("key").Value))
					{
						var dictItem = new Dictionary.DictionaryItem(xmlItem.Attribute("key").Value);
						foreach (var xmlTranslation in xmlItem.Elements())
						{
							var lang = selectedLanguages.Where(l => l.CultureAlias == xmlTranslation.Name).FirstOrDefault();
							if (lang != null)
							{
								if (dictItem.Value(lang.id) != xmlTranslation.Value.Trim())
								{
									if (commit)
										dictItem.setValue(lang.id, xmlTranslation.Value.Trim());
								}
							}
						}
					}
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}