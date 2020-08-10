﻿#region netDxf library, Copyright (C) 2009-2020 Daniel Carvajal (haplokuon@gmail.com)

//                        netDxf library
// Copyright (C) 2009-2020 Daniel Carvajal (haplokuon@gmail.com)
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using netDxf.Collections;
using netDxf.Tables;

namespace netDxf
{
    /// <summary>
    /// Represents the base class for all DXF objects.
    /// </summary>
    public abstract class DxfObject
    {
        #region delegates and events

        public delegate void XDataAddAppRegEventHandler(DxfObject sender, ObservableCollectionEventArgs<ApplicationRegistry> e);

        public event XDataAddAppRegEventHandler XDataAddAppReg;
        protected virtual void OnXDataAddAppRegEvent(ApplicationRegistry item)
        {
            XDataAddAppRegEventHandler ae = this.XDataAddAppReg;
            if (ae != null)
                ae(this, new ObservableCollectionEventArgs<ApplicationRegistry>(item));
        }

        public delegate void XDataRemoveAppRegEventHandler(DxfObject sender, ObservableCollectionEventArgs<ApplicationRegistry> e);
        public event XDataRemoveAppRegEventHandler XDataRemoveAppReg;
        protected virtual void OnXDataRemoveAppRegEvent(ApplicationRegistry item)
        {
            XDataRemoveAppRegEventHandler ae = this.XDataRemoveAppReg;
            if (ae != null)
                ae(this, new ObservableCollectionEventArgs<ApplicationRegistry>(item));
        }

        #endregion

        #region private fields

        private string codename;
        private string handle;
        private DxfObject owner;
        private readonly XDataDictionary xData;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <c>DxfObject</c> class.
        /// </summary>
        /// <param name="codename"><see cref="DxfObjectCode">DXF object name</see>.</param>
        protected DxfObject(string codename)
        {
            this.codename = codename;
            this.handle = null;
            this.owner = null;
            this.xData = new XDataDictionary();
            this.xData.AddAppReg += this.XData_AddAppReg;
            this.xData.RemoveAppReg += this.XData_RemoveAppReg;
        }

        #endregion

        #region public properties

        /// <summary>
        /// Gets the <see cref="DxfObjectCode">DXF object name</see>.
        /// </summary>
        public string CodeName
        {
            get { return this.codename; }
            protected set { this.codename = value; }
        }

        /// <summary>
        /// Gets the handle assigned to the DXF object.
        /// </summary>
        /// <remarks>
        /// The handle is a unique hexadecimal number assigned automatically to every DXF object,
        /// that has been added to a <see cref="DxfDocument">DxfDocument</see>.
        /// </remarks>
        public string Handle
        {
            get { return this.handle; }
            internal set { this.handle = value; }
        }

        /// <summary>
        /// Gets the owner of the actual <see cref="DxfObject">DxfObject</see>.
        /// </summary>
        public DxfObject Owner
        {
            get { return this.owner; }
            internal set { this.owner = value; }
        }

        /// <summary>
        /// Gets the entity <see cref="XDataDictionary">extended data</see>.
        /// </summary>
        public XDataDictionary XData
        {
            get { return this.xData; }
        }

        #endregion

        #region internal methods

        /// <summary>
        /// Assigns a handle to the object based in a integer counter.
        /// </summary>
        /// <param name="entityNumber">Number to assign to the actual object.</param>
        /// <returns>Next available entity number.</returns>
        /// <remarks>
        /// Some objects might consume more than one, is, for example, the case of polylines that will assign
        /// automatically a handle to its vertexes. The entity number will be converted to an hexadecimal number.
        /// </remarks>
        internal virtual long AssignHandle(long entityNumber)
        {
            this.handle = entityNumber.ToString("X");
            return entityNumber + 1;
        }

        #endregion

        #region overrides

        /// <summary>
        /// Obtains a string that represents the DXF object.
        /// </summary>
        /// <returns>A string text.</returns>
        public override string ToString()
        {
            return this.codename;
        }

        #endregion

        #region XData events

        private void XData_AddAppReg(XDataDictionary sender, ObservableCollectionEventArgs<ApplicationRegistry> e)
        {
            this.OnXDataAddAppRegEvent(e.Item);
        }

        private void XData_RemoveAppReg(XDataDictionary sender, ObservableCollectionEventArgs<ApplicationRegistry> e)
        {
            this.OnXDataRemoveAppRegEvent(e.Item);
        }

        #endregion

    }
}