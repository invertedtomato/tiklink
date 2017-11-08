﻿using InvertedTomato.TikLink.RosRecords;
using System.Collections.Generic;
using System;

namespace InvertedTomato.TikLink {
    public class LinkIpDhcpServerAlert {
        private readonly Link Link;

        internal LinkIpDhcpServerAlert(Link link) {
            Link = link;
        }

        public IList<IpDhcpServerAlert> List(string[] properties = null, Dictionary<string, string> filter = null) {
            return Link.List<IpDhcpServerAlert>(properties, filter);
        }

        public IpDhcpServerAlert Get(string id, string[] properties = null) {
            return Link.Get<IpDhcpServerAlert>(id, properties);
        }

        public void Create(IpDhcpServerAlert record, string[] properties = null) {
            Link.Create(record, properties);
        }

        public void Update(IpDhcpServerAlert record, string[] properties = null) {
            Link.Update(record, properties);
        }

        public void Delete(string id) {
            Link.Delete<IpDhcpServerAlert>(id);
        }

        public void Delete(IpDhcpServerAlert record) {
            Link.Delete(record);
        }
    }
}