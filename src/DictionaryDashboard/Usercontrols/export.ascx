<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="export.ascx.cs" Inherits="DictionaryDashboard.usercontrols.export" %>

<div style="padding:15px 15px 10px;">

	<h3 style="margin:5px 0 15px;">Dictionary Export</h3>

	<div style="margin:0 0 20px 0;">
		<div style="margin:0 0 4px 0">Select which languages to include in the export:</div>
		<asp:CheckBoxList runat="server" ID="chkListLanguages" />
	</div>

	<span style="margin-right:10px;"><asp:Button runat="server" ID="btnExport" Text="Export to XML" CausesValidation="false" /></span>

</div>