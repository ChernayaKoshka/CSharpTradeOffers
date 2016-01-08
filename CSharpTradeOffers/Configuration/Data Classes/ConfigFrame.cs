using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// A generic RootConfig object containing configuration information.
    /// </summary>
    [JsonObject(Title = "RootObject")]
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Config")]
    public abstract class ConfigFrame
    {
        /// <summary>
        /// Username to automatically log in to.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password to use to automatically log in.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Special bytes sent to Steam to prove the user is the account holder.
        /// </summary>"
        public string SteamMachineAuth { get; set; }

        /// <summary>
        /// A special key retrieved from https://steamcommunity.com/dev/apikey
        /// The key MUST be from the bot's account.
        /// </summary>
        public string ApiKey { get; set; }

        [XmlArrayItem("element", IsNullable = false)]
        public List<uint> Inventories { get; set; }

        /// <summary>
        /// Initialize all types, allows GenericConfigHandler to properly serialize all fields
        /// </summary>
        public abstract void InitializeAll();
    }
}