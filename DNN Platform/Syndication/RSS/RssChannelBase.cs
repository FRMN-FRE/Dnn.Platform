﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Services.Syndication
{
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    ///   Base class for RSS channel (for strongly-typed and late-bound channel types).
    /// </summary>
    /// <typeparam name = "RssItemType"></typeparam>
    /// <typeparam name = "RssImageType"></typeparam>
    public abstract class RssChannelBase<RssItemType, RssImageType> : RssElementBase
        where RssItemType : RssElementBase, new()
        where RssImageType : RssElementBase, new()
    {
        private readonly List<RssItemType> items = new List<RssItemType>();
        private RssImageType image;
        private string url;

        public List<RssItemType> Items
        {
            get
            {
                return this.items;
            }
        }

        internal string Url
        {
            get
            {
                return this.url;
            }
        }

        public XmlDocument SaveAsXml()
        {
            return this.SaveAsXml(RssXmlHelper.CreateEmptyRssXml());
        }

        public XmlDocument SaveAsXml(XmlDocument emptyRssXml)
        {
            XmlDocument doc = emptyRssXml;
            XmlNode channelNode = RssXmlHelper.SaveRssElementAsXml(doc.DocumentElement, this, "channel");

            if (this.image != null)
            {
                RssXmlHelper.SaveRssElementAsXml(channelNode, this.image, "image");
            }

            foreach (RssItemType item in this.items)
            {
                RssXmlHelper.SaveRssElementAsXml(channelNode, item, "item");
            }

            return doc;
        }

        internal void LoadFromDom(RssChannelDom dom)
        {
            // channel attributes
            this.SetAttributes(dom.Channel);

            // image attributes
            if (dom.Image != null)
            {
                var image = new RssImageType();
                image.SetAttributes(dom.Image);
                this.image = image;
            }

            // items
            foreach (Dictionary<string, string> i in dom.Items)
            {
                var item = new RssItemType();
                item.SetAttributes(i);
                this.items.Add(item);
            }
        }

        protected void LoadFromUrl(string url)
        {
            // download the feed
            RssChannelDom dom = RssDownloadManager.GetChannel(url);

            // create the channel
            this.LoadFromDom(dom);

            // remember the url
            this.url = url;
        }

        protected void LoadFromXml(XmlDocument doc)
        {
            // parse XML
            RssChannelDom dom = RssXmlHelper.ParseChannelXml(doc);

            // create the channel
            this.LoadFromDom(dom);
        }

        protected RssImageType GetImage()
        {
            if (this.image == null)
            {
                this.image = new RssImageType();
            }

            return this.image;
        }
    }
}
