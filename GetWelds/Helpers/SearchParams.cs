using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetWelds.Helpers
{
    public class SearchParam
    {
        /// <summary>
        /// Name of value to be displayed
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description for Parameter
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Regular Expression for Parameter
        /// </summary>
        /// <remarks>This expression should always have 2 values enclosed in parenthesis. the first one is used to return the name for the value property, while the second returns as the actual value.</remarks>
        public string Expression { get; set; }
        /// <summary>
        /// Return Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Filename for parameter
        /// </summary>
        /// <remarks>
        /// This should only be written as a file filter
        ///  i.e. 
        /// *.TP
        /// Definer.tp
        /// </remarks>
        /// <value>*.*</value>
        public string Filename { get; set; }

    }
}
