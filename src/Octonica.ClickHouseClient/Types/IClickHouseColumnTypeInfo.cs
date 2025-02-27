﻿#region License Apache 2.0
/* Copyright 2020-2021 Octonica
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
using System.Text;
using Octonica.ClickHouseClient.Protocol;

namespace Octonica.ClickHouseClient.Types
{
    /// <summary>
    /// Represents basic information about the ClickHouse type. Provides access to factory methods for creating column readers and writers.
    /// </summary>    
    public interface IClickHouseColumnTypeInfo : IClickHouseTypeInfo
    {
        /// <summary>
        /// Creates and returns a new instance of <see cref="IClickHouseColumnReader"/> configured to read the specified number of rows.
        /// </summary>
        /// <param name="rowCount">The number of rows that the reader should read.</param>
        /// <returns>The <see cref="IClickHouseColumnReader"/> that should read the specified number of rows.</returns>
        IClickHouseColumnReader CreateColumnReader(int rowCount);

        /// <summary>
        /// Creates and returns a new instance of <see cref="IClickHouseColumnReaderBase"/> configured to skip the specified number of rows.
        /// </summary>
        /// <param name="rowCount">The number of rows that the reader should skip.</param>
        /// <returns>The <see cref="IClickHouseColumnReaderBase"/> that should skip the specified number of rows.</returns>
        IClickHouseColumnReaderBase CreateSkippingColumnReader(int rowCount);

        /// <summary>
        /// Creates and returns a new instance of <see cref="IClickHouseColumnWriter"/> that can write specified rows to a binary stream.
        /// </summary>
        /// <typeparam name="T">The type of the list of rows.</typeparam>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="rows">The list of rows.</param>
        /// <param name="columnSettings">Optional argument. Additional settings for the column writer.</param>
        /// <returns>The <see cref="IClickHouseColumnWriter"/> that can write specified rows to a binary stream</returns>
        IClickHouseColumnWriter CreateColumnWriter<T>(string columnName, IReadOnlyList<T> rows, ClickHouseColumnSettings? columnSettings);

        /// <summary>
        /// Returns an instance of <see cref="IClickHouseColumnTypeInfo"/> based on this type but with the specified list of type arguments.
        /// </summary>
        /// <param name="options">The list of strings. Each string in the list describes an argument of the type.</param>
        /// <param name="typeInfoProvider">The type provider that can be used to get other types.</param>
        /// <returns>The <see cref="IClickHouseColumnTypeInfo"/> with the same <see cref="IClickHouseTypeInfo.TypeName"/> as this type and with the specified list of type arguments</returns>
        IClickHouseColumnTypeInfo GetDetailedTypeInfo(List<ReadOnlyMemory<char>> options, IClickHouseTypeInfoProvider typeInfoProvider);

        /// <summary>
        /// Appends textual representation of <paramref name="value"/> as ClickHouse SQL literal into <paramref name="queryStringBuilder"/>.
        /// </summary>
        /// <param name="queryStringBuilder">Destination string builder</param>
        /// <param name="value">Value (or null) to be formatted. Value is of compatible type but not necessarily of exact type.</param>
        void FormatValue(StringBuilder queryStringBuilder, object? value);
    }
}
