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

using Octonica.ClickHouseClient.Protocol;

namespace Octonica.ClickHouseClient.Types
{
    internal sealed partial class DateTypeInfo : SimpleTypeInfo
    {
        public DateTypeInfo()
            : base("Date")
        {
        }

        public override IClickHouseColumnReader CreateColumnReader(int rowCount)
        {
            return new DateReader(rowCount);
        }

        public override IClickHouseColumnReaderBase CreateSkippingColumnReader(int rowCount)
        {
            return new SimpleSkippingColumnReader(sizeof(ushort), rowCount);
        }

        public override ClickHouseDbType GetDbType()
        {
            return ClickHouseDbType.Date;
        }

        private sealed partial class DateReader
        {
        }

        private sealed partial class DateWriter
        {
        }
    }
}
