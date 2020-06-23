/*
    Copyright (c) 2020 SaladBadger

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LibDescent.Data;
using Descent2Workshop.Transactions;

namespace Descent2Workshop.EditorPanels
{
    /// <summary>
    /// The pinnacle of UI design, the Reactor Panel™
    /// </summary>
    public partial class ReactorPanel : UserControl
    {
        TransactionManager transactionManager;

        Reactor reactor;
        int reactorID;

        int tabPage;
        public ReactorPanel(TransactionManager transactionManager, int tabPage)
        {
            InitializeComponent();
            this.tabPage = tabPage;
            this.transactionManager = transactionManager;
        }

        public void Init(List<string> modelNames)
        {
            cbReactorModel.Items.Clear();
            cbReactorModel.Items.AddRange(modelNames.ToArray());
        }

        public void Update(Reactor reactor, int id)
        {
            this.reactorID = id;
            this.reactor = reactor;

            cbReactorModel.SelectedIndex = reactor.ModelNum;
        }
    }
}
