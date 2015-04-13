<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="editor.ascx.cs" Inherits="DictionaryDashboard.usercontrols.editor" %>
<style>
    .fixButton{  margin-bottom: 10px;}
</style>
<div style="padding:15px 15px 10px;">

	<h3 style="margin:5px 0 15px;">Dictionary Editor</h3>

	<div style="margin:0 0 20px 0;">
		<label>
			<span style="margin:0 0 4px 0; display:block;">Select which Dictionary item to edit:</span>
			<asp:DropDownList runat="server" ID="selItems" AutoPostBack="true" />
		</label>

        <label>
			<span style="margin:0 0 4px 0; display:block;">Or search by value:</span>
			<asp:TextBox ID="txtTerm" runat="server"></asp:TextBox>
            <asp:Button ID="searchBtn" runat="server" Text="Search" OnClick="searchBtn_Click" CssClass="btn btn-primary fixButton"/>
		</label>
	</div>
	

	<asp:PlaceHolder runat="server" ID="phEdit" Visible="false">
	<div style="margin:-10px 0 10px 0;">
	<asp:Repeater runat="server" ID="repTranslations">
		<ItemTemplate>
			<div style="margin:0 0 10px 0">
				<label>
					<span style="margin:0 0 4px 0; display:block;"><%#language.FriendlyName %></span>
					<textarea name="<%#this.ClientID + "-translation-" + language.id %>" style="width:500px; height:42px; font-family:Verdana; font-size:11px;" rows="2" cols="80"><%#GetValue(language.id) %></textarea>
				</label>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	</div>
	
	<div style="margin:0;">
		<span style="margin-right:10px;"><asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="false" CssClass="btn btn-primary" /></span>
		<span style="margin-right:10px;"><asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" OnClientClick="confirm('Are you sure you want to discard any changes?');" /></span>
	</div>
	</asp:PlaceHolder>


    <asp:PlaceHolder runat="server" ID="phMultipleSearch" Visible="false">
	<div style="margin:-10px 0 10px 0;">
	<asp:Repeater runat="server" ID="repSearch">
		<ItemTemplate>
			<div style="margin:0 0 10px 0">
				<label>
                         <asp:LinkButton ID="lnkbtn" Runat="server" RowID='<%#DataBinder.Eval(Container.DataItem,"UniqueId")%>' OnCommand="clickbutton"><%#Eval("value") %></asp:LinkButton>
				</label>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	</div>
	</asp:PlaceHolder>







	<asp:Literal runat="server" ID="litStatus" />
</div>