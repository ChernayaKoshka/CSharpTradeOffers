using System.Collections.Generic;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Generic command permission class.
    /// </summary>
    public class Command_Permission
    {
        /// <summary>
        /// The exact name of the command.
        /// </summary>
        public string command_name { get; set; }
        /// <summary>
        /// An integer representing a permission level.
        /// </summary>
        public int permission_level { get; set; }
        /// <summary>
        /// Turns the Command_Permission into a KeyValuePair
        /// </summary>
        /// <returns>KeyValuePair</returns>
        public KeyValuePair<string, int> ToKeyValuePair()
        {
            return new KeyValuePair<string, int>(command_name, permission_level);
        }
    }
}