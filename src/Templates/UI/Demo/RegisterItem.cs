using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using N2.Integrity;
using N2.Details;
using N2;
using N2.Web.UI;
using N2.Web.UI.WebControls;

namespace Demo
{
	[Definition("Register for login", "Register")]
    [AllowedZones("Right")]
	[RestrictParents(typeof(N2.Templates.Items.IStructuralPage))]
	[WithEditableTitle("Title", 20)]
    public class RegisterItem : N2.Templates.Items.AbstractItem
	{
		[Editable("Text", typeof(TextBox), "Text", 100)]
		[EditorModifier("TextMode", TextBoxMode.MultiLine)]
		public virtual string Text
		{
			get { return (string)GetDetail("Text"); }
			set { SetDetail("Text", value); }
		}

		[Editable("Submit Text", typeof(TextBox), "Text", 100)]
		[EditorModifier("TextMode", TextBoxMode.MultiLine)]
		public virtual string SubmitText
		{
			get { return (string)GetDetail("SubmitText"); }
			set { SetDetail("SubmitText", value); }
		}

		[Editable("From", typeof(TextBox), "Text", 200)]
		public virtual string From
		{
			get { return (string)GetDetail("From"); }
			set { SetDetail("From", value); }
		}

		[Editable("CC", typeof(TextBox), "Text", 210)]
		//[EditorModifier("Enabled", false)]
		public virtual string CC
		{
			get { return (string)GetDetail("CC"); }
			set { SetDetail("CC", value); }
		}

		[Editable("Subject", typeof(TextBox), "Text", 210)]
		//[EditorModifier("Enabled", false)]
		public virtual string Subject
		{
			get { return (string)GetDetail("Subject"); }
			set { SetDetail("Subject", value); }
		}

		[Editable("Mail Body", typeof(TextBox), "Text", 220)]
		[EditorModifier("TextMode", TextBoxMode.MultiLine)]
		[EditorModifier("Rows", 10)]
		//[EditorModifier("Enabled", false)]
		public virtual string Body
		{
			get { return (string)GetDetail("Body"); }
			set { SetDetail("Body", value); }
		}

		public override string TemplateUrl
		{
			get { return "~/Demo/Register.ascx"; }
		}

		public override string IconUrl
		{
			get
			{
				return "~/Edit/Img/Ico/key.gif";
			}
		}

		public override bool IsPage
		{
			get { return false; }
		}


		//[Editable("Text", typeof(TextBox), "Text", 100)]
		//[EditorModifier("TextMode", TextBoxMode.MultiLine)]
		//public virtual string Text
		//{
		//    get { return (string)GetDetail("Text"); }
		//    set { SetDetail("Text", value); }
		//}

		//[Editable("Submit Text", typeof(TextBox), "Text", 100)]
		//[EditorModifier("TextMode", TextBoxMode.MultiLine)]
		//public virtual string SubmitText
		//{
		//    get { return (string)GetDetail("SubmitText"); }
		//    set { SetDetail("SubmitText", value); }
		//}

		//[Editable("Notification from email", typeof(TextBox), "Text", 200)]
		//public virtual string NotificationFrom
		//{
		//    get { return (string)GetDetail("NotificationFrom"); }
		//    set { SetDetail("NotificationFrom", value); }
		//}

		//[Editable("Notification to email", typeof(TextBox), "Text", 210)]
		//public virtual string NotificationTo
		//{
		//    get { return (string)GetDetail("NotificationTo"); }
		//    set { SetDetail("NotificationTo", value); }
		//}

		////[Editable("Subject", typeof(TextBox), "Text", 210)]
		////public virtual string Subject
		////{
		////    get { return (string)GetDetail("Subject"); }
		////    set { SetDetail("Subject", value); }
		////}

		////[Editable("Mail Body", typeof(TextBox), "Text", 220)]
		////[EditorModifier("TextMode", TextBoxMode.MultiLine)]
		////[EditorModifier("Rows", 10)]
		////public virtual string Body
		////{
		////    get { return (string)GetDetail("Body"); }
		////    set { SetDetail("Body", value); }
		////}

		//public override string TemplateUrl
		//{
		//    get { return "~/Demo/Register.ascx"; }
		//}

		//public override string IconUrl
		//{
		//    get
		//    {
		//        return "~/Edit/Img/Ico/key.gif";
		//    }
		//}

		//public override bool IsPage
		//{
		//    get { return false; }
		//}
	}
}