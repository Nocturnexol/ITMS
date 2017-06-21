using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Data;

namespace Bitshare.PXSM.Common
{
    /// <summary>
    /// DataRow和IDataReader转换实体类
    /// </summary>
    /// <typeparam name="ItemType"></typeparam>
    public class EmitEntityBuilder<ItemType>
    {
        #region 不可改变的参数
        private static readonly MethodInfo getRow =
                typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullRow =
                typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });

        private static readonly MethodInfo getRecord =
                typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullRecord =
                typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
        #endregion

        // 自定义转换实体委托
        public delegate ItemType DynamicMethodDelegate<TParam>(TParam paramObjs);
        static int GetColumnHash(IDataRecord reader)
        {
            unchecked
            {
                int colCount = reader.FieldCount, hash = colCount;
                for (int i = 0; i < colCount; i++)
                {   // binding code is only interested in names - not types
                    object tmp = reader.GetName(i);
                    hash = (hash * 31) + (tmp == null ? 0 : tmp.GetHashCode());
                }
                return hash;
            }
        }

        static Dictionary<int, DynamicMethodDelegate<IDataRecord>> dic = new Dictionary<int, DynamicMethodDelegate<IDataRecord>>();
        private EmitEntityBuilder() { }
        /// <summary>
        /// 创建委托
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static DynamicMethodDelegate<DataRow> CreateHandler(DataRow row)
        {
            System.Type itemType = typeof(ItemType);
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity",
                itemType,
                new Type[] { typeof(DataRow) },
                itemType, true);

            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(itemType);
            generator.Emit(OpCodes.Newobj, itemType.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                PropertyInfo propertyInfo
                    = itemType.GetProperty(row.Table.Columns[i].ColumnName);
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullRow);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getRow);
                    generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            DynamicMethodDelegate<DataRow> handler
                   = (DynamicMethodDelegate<DataRow>)method.CreateDelegate(typeof(DynamicMethodDelegate<DataRow>));
            return handler;
        }
        public static DynamicMethodDelegate<IDataRecord> CreateHandler(IDataRecord dataRecord)
        {
            int hash = GetColumnHash(dataRecord);

            DynamicMethodDelegate<IDataRecord> handler = null;
            if (dic.ContainsKey(hash))
            {
                dic.TryGetValue(hash, out handler);
            }
            else
            {
                System.Type itemType = typeof(ItemType);
                DynamicMethod method = new DynamicMethod("DynamicCreateEntity",
                        itemType,
                        new Type[] { typeof(IDataRecord) },
                        itemType, true);
                ILGenerator generator = method.GetILGenerator();
                LocalBuilder result = generator.DeclareLocal(itemType);
                generator.Emit(OpCodes.Newobj, itemType.GetConstructor(Type.EmptyTypes));
                generator.Emit(OpCodes.Stloc, result);
                for (int i = 0; i < dataRecord.FieldCount; i++)
                {
                    PropertyInfo propertyInfo
                        = itemType.GetProperty(dataRecord.GetName(i));
                    Label endIfLabel = generator.DefineLabel();
                    if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, i);
                        generator.Emit(OpCodes.Callvirt, isDBNullRecord);
                        generator.Emit(OpCodes.Brtrue, endIfLabel);
                        generator.Emit(OpCodes.Ldloc, result);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldc_I4, i);
                        generator.Emit(OpCodes.Callvirt, getRecord);
                        generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                        generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                        generator.MarkLabel(endIfLabel);
                    }
                }
                generator.Emit(OpCodes.Ldloc, result);
                generator.Emit(OpCodes.Ret);

                handler
                      = (DynamicMethodDelegate<IDataRecord>)method.CreateDelegate(typeof(DynamicMethodDelegate<IDataRecord>));
                dic.Add(hash, handler);
            }
            return handler;
        }
    }

}
