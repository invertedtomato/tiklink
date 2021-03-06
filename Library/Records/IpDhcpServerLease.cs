﻿
using System;

namespace InvertedTomato.TikLink.Records {
    /// <summary>
    ///  DHCP server lease submenu is used to monitor and manage server's leases. The issued leases are showed here as dynamic entries. You can also add static leases to issue a specific IP address to a particular client (identified by MAC address) .
    /// 
    /// Generally, the DHCP lease it allocated as follows:
    /// 
    ///     an unused lease is in waiting state
    ///     if a client asks for an IP address, the server chooses one
    ///     if the client receives a statically assigned address, the lease becomes offered, and then bound with the respective lease time
    ///     if the client receives a dynamic address (taken from an IP address pool), the router sends a ping packet and waits for answer for 0.5 seconds. During this time, the lease is marked testing
    ///     in the case where the address does not respond, the lease becomes offered and then bound with the respective lease time
    ///     in other case, the lease becomes busy for the lease time (there is a command to retest all busy addresses), and the client's request remains unanswered (the client will try again shortly) 
    /// 
    /// A client may free the leased address. The dynamic lease is removed, and the allocated address is returned to the address pool. But the static lease becomes busy until the client reacquires the address. 
    /// </summary>
    [RosRecord("/ip/dhcp-server/lease")]
    public class IpDhcpServerLease : SetRecordBase {
        /// <summary>
        /// IP address (or ip pool) for static lease. If set to 0.0.0.0 pool from server will be used
        /// </summary>
        [RosProperty("address")]
        public string/*IP*/ Address { get; set; }

        /// <summary>
        /// Address list to which address will be added if lease is bound.
        /// </summary>
        [RosProperty("address-list")]
        public string AddressList { get; set; }

        /// <summary>
        /// Send all replies as broadcasts
        /// </summary>
        [RosProperty("always-broadcast")]
        public bool AlwaysBroadcast { get; set; }

        /// <summary>
        /// Block access for this client
        /// </summary>
        [RosProperty("block-access")]
        public bool BlockAccess { get; set; }

        /// <summary>
        /// If specified, must match DHCP 'client identifier' option of the request
        /// </summary>
        [RosProperty("client-id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Time that the client may use the address. If set to TimeSpan.Min lease will never expire.
        /// </summary>
        [RosProperty("lease-time")]
        public TimeSpan? LeaseTime { get; set; }

        /// <summary>
        /// If specified, must match the MAC address of the client
        /// </summary>
        [RosProperty("mac-address")]
        public string/*MAC*/ MacAddress { get; set; }

        /// <summary>
        /// Source MAC address
        /// </summary>
        [RosProperty("src-mac-address")]
        public string/*MAC*/ SrcMacAddress { get; set; }

        /// <summary>
        /// Use this source MAC address instead
        /// </summary>
        [RosProperty("use-src-mac")]
        public bool UseSrcMac { get; set; }
        
        /// <summary>
        /// Is this record ignored
        /// </summary>
        [RosProperty("disabled")]
        public bool Disabled { get; set; }

        /// <summary>
        /// Short description of the client
        /// </summary>
        [RosProperty("comment")]
        public string Comment { get; set; }


        /// <summary>
        /// active-address: Actual IP address for this lease
        /// </summary>
        [RosProperty("active-address")] // Read-only
        public string ActiveAddress { get; private set; }

        /// <summary>
        /// active-client-id: Actual client-id of the client
        /// </summary>
        [RosProperty("active-client-id")] // Read-only
        public string ActiveClientId { get; private set; }

        /// <summary>
        /// active-mac-address: Actual MAC address of the client
        /// </summary>
        [RosProperty("active-mac-address")] // Read-only
        public string ActiveMacAddress { get; private set; }

        /// <summary>
        /// active-server: Actual dhcp server, which serves this client
        /// </summary>
        [RosProperty("active-server")] // Read-only
        public string ActiveServer { get; private set; }

        /// <summary>
        /// agent-circuit-id: Circuit ID of DHCP relay agent. If each character should be valid ASCII text symbol or else this value is displayed as hex dump.
        /// </summary>
        [RosProperty("agent-circuit-id")] // Read-only
        public string AgentCircuitId { get; private set; }

        /// <summary>
        /// agent-remote-id: Remote ID, set by DHCP relay agent
        /// </summary>
        [RosProperty("agent-remote-id")] // Read-only
        public string AgentRemoteId { get; private set; }

        /// <summary>
        /// blocked: Whether the lease is blocked
        /// </summary>
        [RosProperty("blocked")] // Read-only
        public string Blocked { get; private set; }

        /// <summary>
        /// expires-after: Time until lease expires
        /// </summary>
        [RosProperty("expires-after")] // Read-only
        public TimeSpan ExpiresAfter { get; private set; }

        /// <summary>
        /// host-name: Shows host name option from last received DHCP request
        /// </summary>
        [RosProperty("host-name")] // Read-only
        public string HostName { get; private set; }

        /// <summary>
        /// radius: Shows if this dynamic lease is authenticated by RADIUS or not
        /// </summary>
        [RosProperty("radius")] // Read-only
        public bool Radius { get; private set; }

        /// <summary>
        /// rate-limit: Sets rate limit for active lease. Format is: rx-rate[/tx-rate] [rx-burst-rate[/tx-burst-rate] [rx-burst-threshold[/tx-burst-threshold] [rx-burst-time[/tx-burst-time]]]]. All rates should be numbers with optional 'k' (1,000s) or 'M' (1,000,000s). If tx-rate is not specified, rx-rate is as tx-rate too. Same goes for tx-burst-rate and tx-burst-threshold and tx-burst-time. If both rx-burst-threshold and tx-burst-threshold are not specified (but burst-rate is specified), rx-rate and tx-rate is used as burst thresholds. If both rx-burst-time and tx-burst-time are not specified, 1s is used as default
        /// </summary>
        [RosProperty("rate-limit")] // Read-only
        public string RateLimit { get; private set; }

        /// <summary>
        /// server: Server name which serves this client
        /// </summary>
        [RosProperty("server")] // Read-only
        public string Server { get; private set; }

        /// <summary>
        /// status
        /// Lease status:
        ///        
        ///               waiting - un-used static lease
        ///               testing - testing whether this address is used or not (only for dynamic leases) by pinging it with timeout of 0.5s 
        ///               authorizing - waiting for response from radius server 
        ///               busy - this address is assigned statically to a client or already exists in the Option, so it can not be leased 
        ///               offered - server has offered this lease to a client, but did not receive confirmation from the client 
        ///               bound - server has received client's confirmation that it accepts offered address, it is using it now and will free the address no later than the lease time 
        ///        
        ///     
        /// </summary>
        [RosProperty("status")] // Read-only
        public string Status { get; private set; } // TODO: Make enum        
    }
}
