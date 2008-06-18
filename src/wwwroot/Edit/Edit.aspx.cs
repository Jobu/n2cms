using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using N2.Definitions;
using N2.Edit.Web;
using N2.Web.UI.WebControls;
using N2.Persistence;

namespace N2.Edit
{
	[NavigationPlugin("Edit", "edit", "../edit.aspx?selected={selected}", "preview", "~/edit/img/ico/page_edit.gif", 20, GlobalResourceClassName = "Navigation")]
	[ToolbarPlugin("", "edit", "edit.aspx?selected={selected}", ToolbarArea.Preview, "preview", "~/Edit/Img/Ico/page_edit.gif", 50, ToolTip = "edit", GlobalResourceClassName = "Toolbar")]
	public partial class Edit : EditPage
	{
		protected PlaceHolder phPluginArea;

		protected bool CreatingNew
		{
			get { return Request["discriminator"] != null; }
		}

		protected override void OnInit(EventArgs e)
		{
			if (Request["cancel"] == "reloadTop")
				hlCancel.NavigateUrl = "javascript:window.top.location.reload();";
			else
				hlCancel.NavigateUrl = Request["returnUrl"] ?? (SelectedItem.VersionOf ?? SelectedItem).Url;

			InitPlugins();
			InitItemEditor();
			InitTitle();
			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			CheckRelatedVersions(ie.CurrentItem);
			
			base.OnPreRender(e);
		}

		private void CheckRelatedVersions(ContentItem item)
		{
			hlNewerVersion.Visible = false;
			hlOlderVersion.Visible = false;

			if (item.VersionOf != null)
			{
				DisplayThisIsVersionInfo(item.VersionOf);
			}
			else
			{
				IList<ContentItem> unpublishedVersions = Find.Items
					.Where.VersionOf.Eq(item)
					.And.Updated.Gt(item.Updated)
					.OrderBy.Updated.Desc.MaxResults(1).Select();

				if(unpublishedVersions.Count > 0 && unpublishedVersions[0].Updated > item.Updated)
				{
					DisplayThisHasNewerVersionInfo(unpublishedVersions[0]);
				}
			}
		}

		private void DisplayThisHasNewerVersionInfo(ContentItem itemToLink)
		{
			string url = Utility.ToAbsolute(Engine.EditManager.GetEditExistingItemUrl(itemToLink));
			hlNewerVersion.NavigateUrl = url;
			hlNewerVersion.Visible = true;
		}

		private void DisplayThisIsVersionInfo(ContentItem itemToLink)
		{
			string url = Utility.ToAbsolute(Engine.EditManager.GetEditExistingItemUrl(itemToLink));
			hlOlderVersion.NavigateUrl = url;
			hlOlderVersion.Visible = true;
		}

		private void InitPlugins()
		{
			foreach(EditToolbarPluginAttribute plugin in Engine.EditManager.GetPlugins<EditToolbarPluginAttribute>(Page.User))
			{
				plugin.AddTo(phPluginArea);
			}
		}

		private void InitTitle()
		{
			if (ie.CurrentItem.ID == 0)
			{
				ItemDefinition definition = Engine.Definitions.GetDefinition(ie.CurrentItemType);
				string definitionTitle = GetGlobalResourceString("Definitions", definition.Discriminator + ".Title") ?? definition.Title;
				string format = GetLocalResourceString("EditPage.TitleFormat.New");
				Title = string.Format(format, definitionTitle);
			}
			else
			{
				string format = GetLocalResourceString("EditPage.TitleFormat.Update");
				Title = string.Format(format, ie.CurrentItem.Title);
			}
		}

		private void InitItemEditor()
		{
			string dataType = Request["dataType"];
			string discriminator = Request["discriminator"];
			if(!string.IsNullOrEmpty(discriminator))
			{
				ie.ItemTypeName = Engine.Definitions.GetDefinition(discriminator).ItemType.AssemblyQualifiedName;
				ie.ParentItemID = SelectedItem.ID;
			}
			else if (!string.IsNullOrEmpty(dataType))
			{
				ie.ItemTypeName = dataType;
				ie.ParentItemID = SelectedItem.ID;
			}
			else
			{
				ie.CurrentItem = SelectedItem;
			}
			ie.ZoneName = base.Page.Request["zoneName"];
		}

		protected override void OnLoad(EventArgs e)
		{
			LoadZones();
			LoadInfo();

			if (!IsPostBack)
				RegisterSetupToolbarScript(SelectedItem);

			base.OnLoad(e);
		}

		private void LoadZones()
		{
			Type itemType = ie.CurrentItemType;
			ucZones.CurrentItem = ie.CurrentItem;
			ItemDefinition definition = N2.Context.Definitions.GetDefinition(itemType);
			ucZones.DataSource = definition.AvailableZones;
			ucZones.DataBind();
		}

		private void LoadInfo()
		{
			ucInfo.CurrentItem = ie.CurrentItem;
			ucInfo.DataBind();
		}

		protected void OnSaveCommand(object sender, CommandEventArgs e)
		{
			Validate();
			if (IsValid)
			{
				ie.VersioningMode = (ie.CurrentItem.VersionOf == null)
					? ItemEditorVersioningMode.VersionAndSave
					: ItemEditorVersioningMode.SaveAsMaster;
				ie.Save();
				ContentItem currentItem = ie.CurrentItem;

				if (Request["before"] != null)
				{
					ContentItem before = Engine.Persister.Get(int.Parse(Request["before"]));
					Engine.Resolve<ITreeSorter>().MoveTo(currentItem, NodePosition.Before, before);
				}
				else if (Request["after"] != null)
				{
					ContentItem after = Engine.Persister.Get(int.Parse(Request["after"]));
					Engine.Resolve<ITreeSorter>().MoveTo(currentItem, NodePosition.After, after);
				}

				Refresh(currentItem.VersionOf ?? currentItem, ToolbarArea.Both);
				Title = string.Format(GetLocalResourceString("SavedFormat"), currentItem.Title);
				ie.Visible = false;
			}
		}

		protected void OnSaveUnpublishedCommand(object sender, CommandEventArgs e)
		{
			Validate();
			if (IsValid)
			{
				ContentItem savedVersion = SaveVersion();
				string redirectUrl = Engine.EditManager.GetEditExistingItemUrl(savedVersion);
				Response.Redirect(redirectUrl);
			}
		}

		protected void OnPreviewCommand(object sender, CommandEventArgs e)
		{
			Validate();
			if (IsValid)
			{
				INode savedVersion = SaveVersion();
				string redirectUrl = savedVersion.PreviewUrl;
				redirectUrl += (redirectUrl.Contains("?") ? "&" : "?") + "preview=true";
				Response.Redirect(redirectUrl);
			}
		}

		private ContentItem SaveVersion()
		{
			ie.VersioningMode = (ie.CurrentItem.VersionOf == null)
				? ItemEditorVersioningMode.VersionOnly
				: ItemEditorVersioningMode.SaveOnly;
			ContentItem savedVersion = ie.Save();
			return savedVersion;
		}
	}
}