using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.multitracks.com.Models
{
    /// <summary>
    /// DTO for Song table in MultiTracksDB.
    /// Casing matches DB columns
    /// </summary>
    public class Song
    {
        public int songID { get; set; }
        public DateTime dateCreation { get; set; }
        public int albumID { get; set; }
        public int artistID { get; set; }
        public string title { get; set; }
        public decimal bpm { get; set; }
        public string timeSignature { get; set; }
        public bool multitracks { get; set; }
        public bool customMix { get; set; }
        public bool chart { get; set; }
        public bool rehearsalMix { get; set; }
        public bool patches { get; set; }
        public bool songSpecificPatches { get; set; }
        public bool proPresenter { get; set; }

    }
}
