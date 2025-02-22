﻿using Azylee.Core.AppUtils.AppConfigUtils.AppConfigInterfaces;
using Azylee.Core.DataUtils.StringUtils;
using Azylee.Core.DbUtils;
using Azylee.Core.DbUtils.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azylee.Core.AppUtils.AppConfigUtils.AppConfigModels
{
    /// <summary>
    /// 数据库配置信息
    /// </summary>
    public class AppConfigDbItem : IAppConfigItemModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 唯一名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 登录用户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 默认数据库
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// 扩展连接字符串
        /// </summary>
        public string ExtConnectString { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 全参数构造函数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        /// <param name="desc"></param>
        public AppConfigDbItem(int number, string type, string name, string server, string port, string userid, string password, string database, string desc)
        {
            Number = number;
            Type = type;
            Name = name;
            Server = server;
            Port = port;
            UserId = userid;
            Password = password;
            Database = database;
            Desc = desc;
        }

        public DatabaseType DbType()
        {
            switch (Type.ToLower())
            {
                case "pg":
                case "postgresql": return DatabaseType.PostgreSQL;

                case "mysql":
                default:
                    return DatabaseType.Mysql;
            }
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="database">执行连接库</param>
        /// <returns></returns>
        public string ConnectionString(string database = null)
        {
            if (!Str.Ok(database)) database = Database;
            switch (DbType())
            {
                case DatabaseType.PostgreSQL:
                    {
                        return $"Server = {Server}; Port = {(Str.Ok(Port) ? Port : "3306")}; User Id = {UserId}; Password = {Password}; Database = {database}; {ExtConnectString}";
                    }

                case DatabaseType.Mysql:
                default:
                    {
                        return $"server = {Server}; port = {(Str.Ok(Port) ? Port : "3306")}; userid = {UserId}; password = {Password}; database = {database}; persistsecurityinfo = True; {ExtConnectString}";
                    }
            }
        }

        /// <summary>
        /// 表空间查询连接字符串
        /// </summary>
        /// <returns></returns>
        public string SchemaConnectionString()
        {
            switch (DbType())
            {
                case DatabaseType.PostgreSQL:
                    {
                        return ConnectionString("");
                    }

                case DatabaseType.Mysql:
                default:
                    {
                        return ConnectionString("information_schema"); ;
                    }
            }
        }
        /// <summary>
        /// 测试连接查询语句
        /// </summary>
        /// <returns></returns>
        public string ValidationQuery()
        {
            switch (DbType())
            {
                case DatabaseType.PostgreSQL:
                case DatabaseType.Mysql:
                default:
                    {

                        return "select now()";
                    }
            }
        }
        /// <summary>
        /// 表空间查询语句
        /// </summary>
        /// <returns></returns>
        public string SchemaQuery()
        {
            switch (DbType())
            {
                case DatabaseType.PostgreSQL:
                    return "SELECT datname AS schema_name FROM pg_database";

                case DatabaseType.Mysql:
                default:
                    return "SELECT schema_name FROM `SCHEMATA`";
            }
        }

        /// <summary>
        /// 排序序号
        /// </summary>
        /// <returns></returns>
        public int GetOrderNumber()
        {
            return this.Number;
        }
        /// <summary>
        /// 唯一名称
        /// </summary>
        /// <returns></returns>
        public string GetUniqueName()
        {
            return this.Name;
        }
    }
}
