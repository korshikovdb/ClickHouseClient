﻿#region License Apache 2.0
/* Copyright 2019-2021 Octonica
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Octonica.ClickHouseClient.Exceptions;
using Octonica.ClickHouseClient.Protocol;

namespace Octonica.ClickHouseClient.Types
{
    internal sealed class Int8TypeInfo : SimpleTypeInfo
    {
        public Int8TypeInfo()
            : base("Int8")
        {
        }

        public override IClickHouseColumnReader CreateColumnReader(int rowCount)
        {
            return new Int8Reader(rowCount);
        }

        public override IClickHouseColumnReaderBase CreateSkippingColumnReader(int rowCount)
        {
            return new SimpleSkippingColumnReader(sizeof(sbyte), rowCount);
        }

        public override IClickHouseColumnWriter CreateColumnWriter<T>(string columnName, IReadOnlyList<T> rows, ClickHouseColumnSettings? columnSettings)
        {
            if (typeof(T) != typeof(sbyte))
                throw new ClickHouseException(ClickHouseErrorCodes.TypeNotSupported, $"The type \"{typeof(T)}\" can't be converted to the ClickHouse type \"{ComplexTypeName}\".");

            return new Int8Writer(columnName, ComplexTypeName, (IReadOnlyList<sbyte>)rows);
        }

        public override void FormatValue(StringBuilder queryStringBuilder, object? value)
        {
            if (value == null || value is DBNull)
                throw new ClickHouseException(ClickHouseErrorCodes.TypeNotSupported, $"The ClickHouse type \"{ComplexTypeName}\" does not allow null values");

            sbyte outputValue;

            if (value is sbyte sbyteValue)
                outputValue = sbyteValue;
            else
                throw new ClickHouseException(ClickHouseErrorCodes.TypeNotSupported, $"The type \"{value.GetType()}\" can't be converted to the ClickHouse type \"{ComplexTypeName}\".");
            
            queryStringBuilder.Append(outputValue.ToString(CultureInfo.InvariantCulture));
        }

        public override Type GetFieldType()
        {
            return typeof(sbyte);
        }

        public override ClickHouseDbType GetDbType()
        {
            return ClickHouseDbType.SByte;
        }

        internal sealed class Int8Reader : StructureReaderBase<sbyte>
        {
            protected override bool BitwiseCopyAllowed => true;

            public Int8Reader(int rowCount)
                : base(sizeof(sbyte), rowCount)
            {
            }

            protected override sbyte ReadElement(ReadOnlySpan<byte> source)
            {
                return unchecked((sbyte) source[0]);
            }

            protected override IClickHouseTableColumn<sbyte> EndRead(ClickHouseColumnSettings? settings, ReadOnlyMemory<sbyte> buffer)
            {
                return new Int8TableColumn(buffer);
            }
        }

        internal sealed class Int8Writer : IClickHouseColumnWriter
        {
            private readonly IReadOnlyList<sbyte> _rows;

            private int _position;

            public string ColumnName { get; }

            public string ColumnType { get; }

            public Int8Writer(string columnName, string columnType, IReadOnlyList<sbyte> rows)
            {
                _rows = rows;
                ColumnName = columnName;
                ColumnType = columnType;
            }

            public SequenceSize WriteNext(Span<byte> writeTo)
            {
                var size = Math.Min(writeTo.Length, _rows.Count - _position);

                for (int i = 0; i < size; i++, _position++)
                    writeTo[i] = unchecked((byte) _rows[_position]);

                return new SequenceSize(size, size);
            }
        }
    }
}
