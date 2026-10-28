<%-- StatusCard.ascx — representative Web Forms user control (reusable UI
     fragment, the Web Forms ancestor of today's design-system component).
     NOT compiled in the .NET 8 solution. --%>
<%@ Control Language="C#" AutoEventWireup="true" %>
<div class="card">
  <h3><asp:Literal ID="TitleLiteral" runat="server" /></h3>
  <p><asp:Literal ID="StatusLiteral" runat="server" /></p>
</div>
