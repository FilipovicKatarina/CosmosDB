#pragma checksum "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "91d2e621f8c58e7deddb02fe1bdd2565b2efc3b8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Korisnik_kreirajKorisnikaa), @"mvc.1.0.view", @"/Views/Korisnik/kreirajKorisnikaa.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\_ViewImports.cshtml"
using MongoDbProj;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\_ViewImports.cshtml"
using MongoDbProj.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"91d2e621f8c58e7deddb02fe1bdd2565b2efc3b8", @"/Views/Korisnik/kreirajKorisnikaa.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"485c6ee1f08021f9b7af648f0212b9358724f6ae", @"/Views/_ViewImports.cshtml")]
    public class Views_Korisnik_kreirajKorisnikaa : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<MongoDbProj.Models.Prodavnica>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("prikaziProizvodeKorisniku"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 3 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml"
  
    ViewData["Title"] = "kreirajKorisnikaa";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1 style=""background-color:darkslategray; color:white; text-align:center"">Prikaz svih prodavnica sa muzickim instrumentima</h1>
<table class=""table"">
    <thead style=""background-color:seagreen"">
        <tr>
        <tr>
            <th width=""200"">Sifra</th>
            <th width=""200"">Naziv</th>
            <th width=""200"">Adresa </th>
            <th width=""200""> Funkcije-proizvodi</th>
        </tr>
        </tr>
    </thead>
    <tbody style=""background-color:lightgreen"">
");
#nullable restore
#line 20 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\n                <td>\n                    ");
#nullable restore
#line 24 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml"
               Write(item.Sifra);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                </td>\n                <td>\n                    ");
#nullable restore
#line 27 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml"
               Write(item.Ime);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                </td>\n                <td>\n                    ");
#nullable restore
#line 30 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml"
               Write(item.Adresa);

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                </td>\n                <td>\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "91d2e621f8c58e7deddb02fe1bdd2565b2efc3b85835", async() => {
                WriteLiteral("\n                        <input type=\"text\" name=\"Id\" placeholder=\"Id\"");
                BeginWriteAttribute("value", " value=\"", 1026, "\"", 1042, 1);
#nullable restore
#line 34 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml"
WriteAttributeValue("", 1034, item.Id, 1034, 8, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" hidden=\"hidden\" />\n                        <input style=\"width:100%;background-color:aliceblue\" color=\"blue\" type=\"submit\" value=\"Pogledaj proizvode\" background-color=\"#C8E0E8\" />\n                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n                </td>\n            </tr>\n");
#nullable restore
#line 39 "C:\Users\User\Downloads\CosmosDB-main\CosmosDB-main\CosmosDB\MongoDbProj\Views\Korisnik\kreirajKorisnikaa.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\n</table>\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<MongoDbProj.Models.Prodavnica>> Html { get; private set; }
    }
}
#pragma warning restore 1591
