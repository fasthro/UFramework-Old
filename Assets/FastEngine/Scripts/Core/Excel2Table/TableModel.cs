/*
 * @Author: fasthro
 * @Date: 2019-12-17 17:34:29
 * @Description: table model
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    /// <summary>
    /// table model interface
    /// </summary>
    public interface ITableObject
    {
        string tableName { get; }
    }
}

