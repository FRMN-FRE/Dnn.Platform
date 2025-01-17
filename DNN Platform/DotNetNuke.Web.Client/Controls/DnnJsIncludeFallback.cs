﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace DotNetNuke.Web.Client.ClientResourceManagement
{
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    ///     Emit a fallback block for a script in the same part of the page.
    /// </summary>
    public class DnnJsIncludeFallback : WebControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DnnJsIncludeFallback"/> class.
        /// </summary>
        /// <param name="objectName">The JS object name to verify is defined in global scope.</param>
        /// <param name="fileName">The path to load if the object is not defined.</param>
        public DnnJsIncludeFallback(string objectName, string fileName)
        {
            this.ObjectName = objectName;
            this.FileName = fileName;
        }

        public string ObjectName { get; set; }

        public string FileName { get; set; }

        /// <inheritdoc/>
        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);

            if (this.ObjectName.Contains("."))
            {
                // generate function check
                writer.Write("if (typeof " + this.ObjectName + " != 'function') {");
            }
            else
            {
                // generate object check
                writer.Write("if (typeof " + this.ObjectName + " == 'undefined') {");
            }

            writer.Write("document.write('<script src=\"" + this.FileName + "\" type=\"text/javascript\"></' + 'script>');");
            writer.Write("}");
            writer.RenderEndTag();
        }
    }
}
