﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace OFX {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class STATUS {
        
        private string cODEField;
        
        private STATUSSEVERITY sEVERITYField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="integer")]
        public string CODE {
            get {
                return this.cODEField;
            }
            set {
                this.cODEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public STATUSSEVERITY SEVERITY {
            get {
                return this.sEVERITYField;
            }
            set {
                this.sEVERITYField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum STATUSSEVERITY {
        
        /// <remarks/>
        INFO,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class OFX {
        
        private OFXSIGNONMSGSRSV1 sIGNONMSGSRSV1Field;
        
        private OFXBANKMSGSRSV1 bANKMSGSRSV1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXSIGNONMSGSRSV1 SIGNONMSGSRSV1 {
            get {
                return this.sIGNONMSGSRSV1Field;
            }
            set {
                this.sIGNONMSGSRSV1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1 BANKMSGSRSV1 {
            get {
                return this.bANKMSGSRSV1Field;
            }
            set {
                this.bANKMSGSRSV1Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXSIGNONMSGSRSV1 {
        
        private OFXSIGNONMSGSRSV1SONRS sONRSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXSIGNONMSGSRSV1SONRS SONRS {
            get {
                return this.sONRSField;
            }
            set {
                this.sONRSField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXSIGNONMSGSRSV1SONRS {
        
        private STATUS sTATUSField;
        
        private string dTSERVERField;
        
        private OFXSIGNONMSGSRSV1SONRSLANGUAGE lANGUAGEField;
        
        /// <remarks/>
        public STATUS STATUS {
            get {
                return this.sTATUSField;
            }
            set {
                this.sTATUSField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DTSERVER {
            get {
                return this.dTSERVERField;
            }
            set {
                this.dTSERVERField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXSIGNONMSGSRSV1SONRSLANGUAGE LANGUAGE {
            get {
                return this.lANGUAGEField;
            }
            set {
                this.lANGUAGEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum OFXSIGNONMSGSRSV1SONRSLANGUAGE {
        
        /// <remarks/>
        ENG,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXBANKMSGSRSV1 {
        
        private OFXBANKMSGSRSV1STMTTRNRS sTMTTRNRSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRS STMTTRNRS {
            get {
                return this.sTMTTRNRSField;
            }
            set {
                this.sTMTTRNRSField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXBANKMSGSRSV1STMTTRNRS {
        
        private int tRNUIDField;
        
        private STATUS sTATUSField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRS sTMTRSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int TRNUID {
            get {
                return this.tRNUIDField;
            }
            set {
                this.tRNUIDField = value;
            }
        }
        
        /// <remarks/>
        public STATUS STATUS {
            get {
                return this.sTATUSField;
            }
            set {
                this.sTATUSField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRS STMTRS {
            get {
                return this.sTMTRSField;
            }
            set {
                this.sTMTRSField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXBANKMSGSRSV1STMTTRNRSSTMTRS {
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSCURDEF cURDEFField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROM bANKACCTFROMField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLIST bANKTRANLISTField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSLEDGERBAL lEDGERBALField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRSCURDEF CURDEF {
            get {
                return this.cURDEFField;
            }
            set {
                this.cURDEFField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROM BANKACCTFROM {
            get {
                return this.bANKACCTFROMField;
            }
            set {
                this.bANKACCTFROMField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLIST BANKTRANLIST {
            get {
                return this.bANKTRANLISTField;
            }
            set {
                this.bANKTRANLISTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRSLEDGERBAL LEDGERBAL {
            get {
                return this.lEDGERBALField;
            }
            set {
                this.lEDGERBALField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum OFXBANKMSGSRSV1STMTTRNRSSTMTRSCURDEF {
        
        /// <remarks/>
        GBP,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROM {
        
        private string bANKIDField;
        
        private string aCCTIDField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE aCCTTYPEField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BANKID {
            get {
                return this.bANKIDField;
            }
            set {
                this.bANKIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ACCTID {
            get {
                return this.aCCTIDField;
            }
            set {
                this.aCCTIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE ACCTTYPE {
            get {
                return this.aCCTTYPEField;
            }
            set {
                this.aCCTTYPEField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE {
        
        /// <remarks/>
        CHECKING,
        
        /// <remarks/>
        SAVINGS,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLIST {
        
        private string dTSTARTField;
        
        private string dTENDField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN[] sTMTTRNField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DTSTART {
            get {
                return this.dTSTARTField;
            }
            set {
                this.dTSTARTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DTEND {
            get {
                return this.dTENDField;
            }
            set {
                this.dTENDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("STMTTRN", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN[] STMTTRN {
            get {
                return this.sTMTTRNField;
            }
            set {
                this.sTMTTRNField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN {
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE tRNTYPEField;
        
        private string dTPOSTEDField;
        
        private decimal tRNAMTField;
        
        private string fITIDField;
        
        private string nAMEField;
        
        private string mEMOField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE TRNTYPE {
            get {
                return this.tRNTYPEField;
            }
            set {
                this.tRNTYPEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DTPOSTED {
            get {
                return this.dTPOSTEDField;
            }
            set {
                this.dTPOSTEDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal TRNAMT {
            get {
                return this.tRNAMTField;
            }
            set {
                this.tRNAMTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FITID {
            get {
                return this.fITIDField;
            }
            set {
                this.fITIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NAME {
            get {
                return this.nAMEField;
            }
            set {
                this.nAMEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MEMO {
            get {
                return this.mEMOField;
            }
            set {
                this.mEMOField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE {
        
        /// <remarks/>
        ATM,
        
        /// <remarks/>
        CHECK,
        
        /// <remarks/>
        CREDIT,
        
        /// <remarks/>
        DEBIT,
        
        /// <remarks/>
        DIRECTDEBIT,
        
        /// <remarks/>
        REPEATPMT,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXBANKMSGSRSV1STMTTRNRSSTMTRSLEDGERBAL {
        
        private decimal bALAMTField;
        
        private string dTASOFField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal BALAMT {
            get {
                return this.bALAMTField;
            }
            set {
                this.bALAMTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DTASOF {
            get {
                return this.dTASOFField;
            }
            set {
                this.dTASOFField = value;
            }
        }
    }
}
