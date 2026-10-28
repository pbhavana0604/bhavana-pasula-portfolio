<%-- BenefitsLookup.aspx — REPRESENTATIVE .NET Framework Web Forms sample.
     NOT compiled in the .NET 8 solution (Web Forms is .NET Framework only).
     Fictional demo data only. See docs/LEGACY_MODERNIZATION.md. --%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BenefitsLookup.aspx.cs" Inherits="LegacyDemo.BenefitsLookup" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server"><title>Benefits lookup (legacy example)</title></head>
<body>
<form id="form1" runat="server">
  <div>
    <asp:Label ID="ReferenceLabel" runat="server" AssociatedControlID="ReferenceTextBox" Text="Reference number" />
    <asp:TextBox ID="ReferenceTextBox" runat="server" />
    <asp:RequiredFieldValidator ID="ReferenceValidator" runat="server"
      ControlToValidate="ReferenceTextBox" ErrorMessage="Enter a reference number." Display="Dynamic" />
    <asp:Button ID="LookupButton" runat="server" Text="Look up" OnClick="LookupButton_Click" />
    <asp:Label ID="ResultLabel" runat="server" />
  </div>
</form>
</body>
</html>
