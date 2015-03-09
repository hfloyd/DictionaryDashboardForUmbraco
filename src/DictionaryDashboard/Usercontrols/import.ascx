<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="import.ascx.cs" Inherits="DictionaryDashboard.usercontrols.import" %>

<div style="padding:15px 15px 10px;">
<asp:Label runat="server" ID="lbl"></asp:Label>
	<h3 style="margin:5px 0 15px;">Dictionary Import</h3>

	<div style="margin:0 0 20px 0;">
		<div style="margin:0 0 4px 0">Choose an XML file to import. This will update the Dictionary.</div>
		<div style="margin:0 0 8px 0">
			<asp:FileUpload runat="server" ID="fileUploadXml" CausesValidation="false" />
		</div>

		<div style="margin:0 0 4px 0">Select which languages to include in the import:</div>
		<div style="margin:0 0 8px 0">
			<asp:CheckBoxList runat="server" ID="chkListLanguages" />
		</div>

		<span style="margin-right:10px;"><asp:Button runat="server" ID="btnValidate" Text="Validate XML" CausesValidation="false" /></span>
		<span style="margin-right:10px;"><asp:Button runat="server" ID="btnImport" Text="Import XML" CausesValidation="false" /></span>
	</div>

	<asp:Literal runat="server" ID="litStatus" />

</div>