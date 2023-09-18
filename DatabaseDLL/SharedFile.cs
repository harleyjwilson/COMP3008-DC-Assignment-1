/*
 * Class to store shared file
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL {
    [DataContractAttribute]
    public class SharedFile {

        public SharedFile(string fileName, string fileType) {
            this.FileName = fileName;
            this.FileType = fileType;
        }
        [DataMemberAttribute()]
        public string FileName { get; set; }
        [DataMemberAttribute()]
        public string FileType { get; set; } // e.g., "image", "text"
        [DataMemberAttribute()]
        public byte[] FileData { get; set; }
    }
}
