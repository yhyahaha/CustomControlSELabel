﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Interfaces
{
    public interface ISELabelItem
    {       
        /// <summary>
        /// オブジェクトのID, MasterCode, StringValue, DateTimeValue etc..
        /// </summary>
        object ItemValue { get; set; }

        /// <summary>
        /// SELabelにObjectが設定されている時に表示される文字列
        /// </summary>
        string DisplayString { get; set; }

        /// <summary>
        /// 検索結果がリスト表示された場合のソート
        /// 通常はヨミガナを使う
        /// </summary>
        string SortKey { get; set; }

        /// <summary>
        /// 検索に使うキーワード
        /// スペース１文字 + キーワードの羅列
        /// Containsで中間一致、[Space]キーワードで前方一致に対応する
        /// </summary>
        string SearchKeys { get; set; }

        /// <summary>
        /// DisplayStringのフォーマットに利用
        /// </summary>
        Type DataType { get; set; }
    }
}
