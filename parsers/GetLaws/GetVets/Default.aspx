<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GetVets._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Modify this template to jump-start your ASP.NET application.</h2>
            </hgroup>
            <p>
                To learn more about ASP.NET, visit <a href="http://asp.net" title="ASP.NET Website">http://asp.net</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET.
                If you have any questions about ASP.NET visit
                <a href="http://forums.asp.net/18.aspx" title="ASP.NET Forum">our forums</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:Button ID="GetVets" runat="server" Text="GetVets" OnClick="GetVets_Click" />
    <asp:TextBox ID="Result" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="GetLaws" runat="server" OnClick="GetLaws_Click" Text="GetLaws" />
    <asp:TextBox ID="ResultLaws" runat="server"></asp:TextBox>
    <asp:Label ID="Label1" runat="server" Text="Status: "></asp:Label>
    <asp:Label ID="lblProgress" runat="server"></asp:Label>
</asp:Content>
