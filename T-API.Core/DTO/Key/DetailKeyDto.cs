﻿namespace T_API.Core.DTO.Key
{
    public class DetailKeyDto
    {
        public string KeyName { get; set; }
        public string KeyColumn { get; set; }
        public bool IsPrimary { get; set; }

        public string TableName { get; set; }
    }
}