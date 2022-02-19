﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PISservice.Model
{
    public class ClientFileModel
    {
        public Guid requestID { get; set; }
        public string fileOwner { get; set; }
        public string fileType { get; set; }
        public string fileName { get; set; }
        public string fileExtension { get; set; }
        public byte[] fajl { get; set; }
    }
}
