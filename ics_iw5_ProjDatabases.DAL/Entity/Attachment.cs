﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ics_iw5_ProjDatabases.DAL.Entity
{   [Table("Attachment")]
    class Attachment
    {
        [Key]
        [Column]
        public int AttachmentID { get; set; }
        [Column]
        public byte[] File { get; set; }
        [Column]
        public int Members { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public DateTime Time { get; set; }
    }
}
