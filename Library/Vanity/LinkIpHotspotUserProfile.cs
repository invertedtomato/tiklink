﻿using InvertedTomato.TikLink.Records;
using System.Collections.Generic;
using System;

namespace InvertedTomato.TikLink.Vanity {
    public class LinkIpHotspotUserProfile {
        private readonly Link Link;

        internal LinkIpHotspotUserProfile(Link link) {
            Link = link;
        }

        public IList<IpHotspotUserProfile> List(string[] properties = null, Dictionary<string, string> filter = null) {
            return Link.List<IpHotspotUserProfile>(properties, filter);
        }

        public IpHotspotUserProfile Get(string id, string[] properties = null) {
            return Link.Get<IpHotspotUserProfile>(id, properties);
        }

        public void Add(IpHotspotUserProfile record, string[] properties = null) {
            Link.Add(record, properties);
        }

        public void Update(IpHotspotUserProfile record, string[] properties = null) {
            Link.Update(record, properties);
        }

        public void Delete(string id) {
            Link.Delete<IpHotspotUserProfile>(id);
        }

        public void Delete(IpHotspotUserProfile record) {
            Link.Delete(record);
        }
    }
}