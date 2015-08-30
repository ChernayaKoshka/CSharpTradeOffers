using System.Collections.Generic;

namespace CSharpTradeOffers.Configuration
{
    /// <summary>
    /// Class containing Dictionary versions of Offer/CommandPermission
    /// </summary>
    public class Dictionaries
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<ulong, int> Officers_Dict = new Dictionary<ulong, int>();
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, int> Command_Permissions_Dict = new Dictionary<string, int>();
        /// <summary>
        /// Clear officers dictionary. This should not be called directly.
        /// </summary>
        public void ClearOfficer()
        {
            Officers_Dict.Clear();
        }
        /// <summary>
        /// Clear permissions dictionary. This shold not be called directly.
        /// </summary>
        public void ClearPerms()
        {
            Command_Permissions_Dict.Clear();
        }
    }
}