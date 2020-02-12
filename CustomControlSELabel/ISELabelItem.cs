using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControlSELabel
{
    public interface ISELabelItem
    {       
        /// <summary>
        /// オブジェクトのID
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// SELabelにObjectが設定されている時に表示される文字列
        /// </summary>
        public string DisplayString { get; }

        /// <summary>
        /// 検索結果がリスト表示された場合のソート
        /// 通常はヨミガナを使う
        /// </summary>
        public string SortKey { get; }

        /// <summary>
        /// 検索に使うキーワード
        /// スペース１文字 + キーワードの羅列
        /// Containsで中間一致、[Space]キーワードで前方一致に対応する
        /// </summary>
        public string SearchKeys { get; }

        /// <summary>
        /// SELabelItemのデータタイプ
        /// </summary>
        public SELableDataType DataType { get; }
        
    }
}
