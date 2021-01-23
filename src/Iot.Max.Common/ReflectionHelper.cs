/**
 * 业务中反射帮助类
 * Date：2020/10/30
 * Author：Max
 * Function：
 *  1、DataTable和List互转
 *  2、Model类转插入、更新Sql语句
 * 
 * **/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Iot.Max.Common
{
    public class ReflectionLib
    {
        public static Tuple<bool, string, List<T>> DataTableToList<T>(DataTable dt)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                Type t = typeof(T);
                PropertyInfo[] p = t.GetProperties();
                List<T> list = new List<T>();
                foreach (DataRow dr in dt.Rows)
                {
                    T obj = (T)Activator.CreateInstance(t);

                    string[] sdrFileName = new string[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sdrFileName[i] = dt.Columns[i].ColumnName.ToLower();
                    }
                    foreach (PropertyInfo item in p)
                    {
                        if (sdrFileName.ToList().IndexOf(item.Name.ToLower()) > -1)
                        {
                            if (dr[item.Name] != System.DBNull.Value)
                            {
                                item.SetValue(obj, dr[item.Name]);
                            }
                            else
                            {
                                item.SetValue(obj, null);
                            }
                        }
                        else
                        {
                            item.SetValue(obj, null);
                        }
                    }
                    list.Add(obj);
                }
                dto.Data = list;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

        public static Tuple<string, DataTable> ListToDataTable<T>(List<T> ts)
        {
            return null;
        }

        /// <summary>
        /// dapper insert sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="tableName"></param>
        /// <returns>bool(是否成功),string(文本信息),string(sql语句)</returns>
        public static Tuple<bool, string, string> ModelToDapperInsertSql<T>(T t, string tableName="")
        {
            LibResultDto dto = new LibResultDto();
            StringBuilder _strField = new StringBuilder();
            StringBuilder _strValue = new StringBuilder();
            try
            {
                #region get model attribute
                MemberInfo info = typeof(T);
                var tas = (TableAttribute)info.GetCustomAttribute(typeof(TableAttribute), false);

                if (tas != null)
                    tableName = tas.Name;

                if (string.IsNullOrEmpty(tableName)) { 
                    return Tuple.Create(false, "表名不存在", "");
                }

                #endregion

                PropertyInfo[] PropertyInfos = t.GetType().GetProperties();
                foreach (PropertyInfo pi in PropertyInfos)
                {
                    object _objValue = pi.GetValue(t, null);
                    Type _type = pi.PropertyType;
                    if (_objValue != null)
                    {
                        switch (_type.Name.ToLower())
                        {
                            case "datetime":
                                string _date = Convert.ToDateTime(_objValue).ToString("yyyy-MM-dd HH:mm:ss");
                                if (!_date.Equals("0001-01-01 00:00:00"))
                                {
                                    _strField.Append($"{pi.Name},");
                                    _strValue.Append($"@{pi.Name},");
                                }
                                break;
                            case "guid":
                                //if (!Convert.ToString(_objValue).Equals("00000000-0000-0000-0000-000000000000"))
                                Guid? guid = Guid.Parse(Convert.ToString(_objValue));
                                if (guid != null && guid.HasValue)
                                {
                                    _strField.Append($"{pi.Name},");
                                    _strValue.Append($"@{pi.Name},");
                                }
                                break;
                            default:
                                _strField.Append($"{pi.Name},");
                                _strValue.Append($"@{pi.Name},");
                                break;
                        }
                    }
                }
                string _strSql = $"insert into {tableName}({_strField.ToString().TrimEnd(',')}) values({_strValue.ToString().TrimEnd(',')})";
                dto.Data = _strSql;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }
        public static Tuple<bool, string, string> ModelToAdoInsertSql<T>(T t, string tableName, string fieldKey)
        {
            LibResultDto dto = new LibResultDto();
            string _strField = "";
            string _strValue = "";
            string _strSql;
            try
            {
                PropertyInfo[] PropertyInfos = t.GetType().GetProperties();
                foreach (PropertyInfo pi in PropertyInfos)
                {
                    object _objValue = pi.GetValue(t, null);
                    Type _type = pi.PropertyType;
                    if (_objValue != null)
                    {
                        if (pi.Name.ToLower() != fieldKey.ToLower())
                        {
                            switch (_type.Name.ToLower())
                            {
                                case "datetime":
                                    string _date = Convert.ToDateTime(_objValue).ToString("yyyy-MM-dd HH:mm:ss");
                                    if (!_date.Equals("0001-01-01 00:00:00"))
                                    {
                                        _strField += pi.Name + ",";
                                        _strValue += string.Format("'{0}',", _date);
                                    }
                                    break;
                                case "guid":
                                    if (!Convert.ToString(_objValue).Equals("00000000-0000-0000-0000-000000000000"))
                                    {
                                        _strField += pi.Name + ",";
                                        _strValue += string.Format("'{0}',", _objValue);
                                    }
                                    break;
                                case "string":
                                    _strField += pi.Name + ",";
                                    _strValue += string.Format("N'{0}',", _objValue == null ? "" : _objValue.ToString().Replace("'", "''"));
                                    break;
                                default:
                                    _strField += pi.Name + ",";
                                    _strValue += string.Format("'{0}',", _objValue == null ? "" : _objValue.ToString().Replace("'", "''"));
                                    break;
                            }
                        }
                    }
                }
                _strField = _strField.TrimEnd(',');
                _strValue = _strValue.TrimEnd(',');
                _strSql = $"insert into {tableName}({_strField}) values({_strValue})";
                dto.Data = _strSql;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

        public static Tuple<bool, string, string> ModelToSelectSql<T>(string tableName = "") 
        {
            LibResultDto dto = new LibResultDto();
            StringBuilder _strField = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    var tn = GetAttributeTableName<T>();

                    if(!string.IsNullOrEmpty(tn))
                    {
                        tableName = tn;
                    }

                    if (string.IsNullOrEmpty(tableName))
                    {
                        return Tuple.Create(false, "表名参数不正确/特性表名不存在", "");
                    }
                }
                
                PropertyInfo[] PropertyInfos = typeof(T).GetProperties();
                foreach (PropertyInfo pi in PropertyInfos)
                {
                    _strField.Append($"{pi.Name},");
                }
                string _strSql = $"select {_strField.ToString().TrimEnd(',')} from {tableName}";
                dto.Data = _strSql;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }


        public static Tuple<bool, string, string> ModelToUpdateSql<T>(T t, string tableName = "", string sKey = "")
        {
            LibResultDto dto = new LibResultDto();
            string _strValue = "";
            string _strSet = "";
            string _strWhere = "";
            string _strSql;

            sKey = sKey?.ToLower() ?? "id";
            tableName = tableName ?? t.GetType().Name;
            try
            {
                PropertyInfo[] PropertyInfos = t.GetType().GetProperties();
                foreach (PropertyInfo pi in PropertyInfos)
                {
                    string _strField = pi.Name;
                    object _objValue = pi.GetValue(t, null);
                    Type _type = pi.PropertyType;

                    if (_objValue != null && !pi.Name.ToLower().Equals(sKey))
                    {
                        switch (_type.Name.ToLower())
                        {
                            case "datetime":
                                _strValue = Convert.ToDateTime(_objValue).ToString("yyyy-MM-dd HH:mm:ss");
                                if (!_strValue.Equals("0001-01-01 00:00:00"))
                                    _strSet += string.Format("{0} = '{1}',", _strField, _strValue);
                                break;
                            case "guid":
                                if (!Convert.ToString(_objValue).Equals("00000000-0000-0000-0000-000000000000"))
                                    _strValue += string.Format("'{0}',", _objValue);
                                break;
                            default:
                                _strValue = string.Format("{0}", _objValue);
                                if (_strField.ToLower() == sKey.ToLower())
                                {
                                    _strWhere = string.Format(" and {0} = '{1}'", _strField, _strValue);
                                }
                                else
                                {
                                    _strSet += string.Format("{0} = N'{1}',", _strField, _strValue);
                                }
                                break;
                        }
                    }
                    else if (_objValue != null && pi.Name.ToLower().Equals(sKey))
                    {
                        _strWhere = string.Format(" and {0} = '{1}'", _strField, _objValue);
                    }

                }
                _strSet = _strSet.TrimEnd(',');
                _strSql = $"update {tableName} set {_strSet} where 1 = 1 {_strWhere}";
                dto.Data = _strSql;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }
        public static Tuple<bool, string, string> ModelToDapperUpdateSql<T>(T t, string tableName = "", string sKey = "id")
        {
            LibResultDto dto = new LibResultDto();
            string _strValue = "";
            StringBuilder _strSet = new StringBuilder();
            StringBuilder _strWhere = new StringBuilder();
            string _strSql;

            sKey = sKey?.ToLower() ?? "id";

            #region get model attribute
            MemberInfo info = typeof(T);
            var tas = (TableAttribute)info.GetCustomAttribute(typeof(TableAttribute), false);

            if (tas != null)
                tableName = tas.Name;

            if (string.IsNullOrEmpty(tableName))
            {
                return Tuple.Create(false, "表名不存在", "");
            }

            #endregion

            try
            {
                PropertyInfo[] PropertyInfos = t.GetType().GetProperties();
                foreach (PropertyInfo pi in PropertyInfos)
                {
                    string _strField = pi.Name;
                    object _objValue = pi.GetValue(t, null);
                    Type _type = pi.PropertyType;

                    if (_objValue != null && !pi.Name.ToLower().Equals(sKey))
                    {
                        switch (_type.Name.ToLower())
                        {
                            case "datetime":
                                _strValue = Convert.ToDateTime(_objValue).ToString("yyyy-MM-dd HH:mm:ss");
                                if (!_strValue.Equals("0001-01-01 00:00:00"))
                                    _strSet.Append($"{_strField} = @{_strField},");
                                break;
                            case "guid":
                                Guid? guid = Guid.Parse(Convert.ToString(_objValue));
                                if (guid != null && guid.HasValue)
                                    _strSet.Append($"{_strField} = @{_strField},");
                                break;
                            default:
                                if (!_strField.ToLower().Equals(sKey.ToLower()))
                                {
                                    _strSet.Append($"{_strField} = @{_strField},");
                                }
                                break;
                        }
                    }
                    else if (_objValue != null && pi.Name.ToLower().Equals(sKey))
                    {
                        _strWhere.Append($"{_strField} = @{_strField}");
                    }

                }
                _strSql = $"update {tableName} set {_strSet.ToString().TrimEnd(',')} where {_strWhere}";
                dto.Data = _strSql;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

        public static string GetAttributeTableName<T>()
        {
            string _tableName = "";
            MemberInfo info = typeof(T);
            var tas = (TableAttribute)info.GetCustomAttribute(typeof(TableAttribute), false);

            if (tas != null)
                _tableName = tas.Name;

            return _tableName;
        }
    }
}
