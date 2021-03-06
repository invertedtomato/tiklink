﻿namespace InvertedTomato.TikLink.Records {
    /// <summary>
    /// ip/arp: Even though IP packets are addressed using  IP addresses, hardware addresses must be used to actually transport data from one host to another.Address Resolution Protocol is used to map OSI level 3 IP addresses to OSI level 2 MAC addreses. Router has a table of currently used ARP entries.Normally the table is built dynamically, but to increase Option security, it can be partialy or completely built statically by means of adding static entries.
    /// </summary>
    [RosRecord("/ip/arp")]
    public class IpArp : SetRecordBase {
        /// <summary>
        /// IP Address
        /// </summary>
        [RosProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Interface name the IP address is assigned to
        /// </summary>
        [RosProperty("interface")]
        public string Interface { get; set; }

        /// <summary>
        /// MAC address to be mapped to
        /// </summary>
        [RosProperty("mac-address")]
        public string MacAddress { get; set; }

        /// <summary>
        /// IP Address
        /// </summary>
        [RosProperty("comment")]
        public string Comment { get; set; }


        /// <summary>
        /// Whether ARP entry is added by DHCP server
        /// </summary>
        [RosProperty("dhcp")] // Read-only
        public bool Dhcp { get; private set; }

        /// <summary>
        /// Whether entry is dynamically created
        /// </summary>
        [RosProperty("dynamic")] // Read-only
        public bool Dynamic { get; private set; }

        /// <summary>
        /// Whether entry is not valid
        /// </summary>
        [RosProperty("invalid")] // Read-only
        public bool Invalid { get; private set; }

        public override string ToString() {
            return $"{Address}=>{MacAddress}";
        }
    }

}
