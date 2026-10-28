// BenefitsLookup.aspx.cs — .NET Framework Web Forms code-behind (NOT compiled here).
// Postback model: every button click posts the whole page back, ViewState
// round-trips control state, and the handler re-renders the same page.
// Modern equivalent: GET /api/applications/{reference} in this repo's
// ApplicationsController, called from a static page — no postback, no ViewState.
using System;
using System.Web.UI;

namespace LegacyDemo
{
    public partial class BenefitsLookup : Page
    {
        protected void LookupButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            var reference = ReferenceTextBox.Text.Trim();
            // In a maintained legacy app this would call the same repository
            // pattern as the modern code, with a parameterized query — never
            // string concatenation. Kept deliberately simple for the sample:
            ResultLabel.Text = "Demo lookup for " + Server.HtmlEncode(reference)
                + " (fictional record; see the modern API for the real flow).";
        }
    }
}
