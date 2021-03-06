﻿using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bitshare.DataDecision.Model
{
    public class PhysicalDevice
    {
        public ObjectId _id { get; set; }
        public int Rid { get; set; }
        public string ModelNum { get; set; }
        public int? Owner { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true,NullDisplayText = "")]
        public DateTime? Date { get; set; }
        public string PublicIP { get; set; }
        public string IntranetIP { get; set; }
        public string Remark { get; set; }
        public int? DeviceType { get; set; }
        public string Cpu { get; set; }
        public string Memory { get; set; }
        public string Storage { get; set; }
        public string Locale { get; set; }
         [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? WarrantyExpiry { get; set; }
         [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? PurchaseDate { get; set; }
        //[RegularExpression("^((2[0-4]\\d|25[0-5]|[1-9]?\\d|1\\d{2})\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$", ErrorMessage = "无效的IP地址")]
        public string ManagementIP { get; set; }
        public string NetTpParam { get; set; }
        public string MachineName { get; set; }
        public string DomainIP { get; set; }

    }
}
