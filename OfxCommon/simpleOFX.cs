﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
namespace SimpleOfx {
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
    public partial class LedgerBalType {
        
        private string bALAMTField;
        
        private string dTASOFField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BALAMT {
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BankTranListType {
        
        private string dTSTARTField;
        
        private string dTENDField;
        
        private System.ComponentModel.BindingList<BankTranListTypeSTMTTRN> sTMTTRNField;
        
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
        public System.ComponentModel.BindingList<BankTranListTypeSTMTTRN> STMTTRN {
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
    public partial class BankTranListTypeSTMTTRN {
        
        private BankTranListTypeSTMTTRNTRNTYPE tRNTYPEField;
        
        private string dTPOSTEDField;
        
        private string dTUSERField;
        
        private string dTAVAILField;
        
        private string tRNAMTField;
        
        private string fITIDField;
        
        private string rEFNUMField;
        
        private string nAMEField;
        
        private string mEMOField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BankTranListTypeSTMTTRNTRNTYPE TRNTYPE {
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
        public string DTUSER {
            get {
                return this.dTUSERField;
            }
            set {
                this.dTUSERField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string DTAVAIL {
            get {
                return this.dTAVAILField;
            }
            set {
                this.dTAVAILField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TRNAMT {
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
        public string REFNUM {
            get {
                return this.rEFNUMField;
            }
            set {
                this.rEFNUMField = value;
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
    public enum BankTranListTypeSTMTTRNTRNTYPE {
        
        /// <remarks/>
        CREDIT,
        
        /// <remarks/>
        DEBIT,
        
        /// <remarks/>
        INT,
        
        /// <remarks/>
        DIV,
        
        /// <remarks/>
        FEE,
        
        /// <remarks/>
        SRVCHG,
        
        /// <remarks/>
        DEP,
        
        /// <remarks/>
        ATM,
        
        /// <remarks/>
        POS,
        
        /// <remarks/>
        XFER,
        
        /// <remarks/>
        CHECK,
        
        /// <remarks/>
        PAYMENT,
        
        /// <remarks/>
        CASH,
        
        /// <remarks/>
        DIRECTDEP,
        
        /// <remarks/>
        DIRECTDEBIT,
        
        /// <remarks/>
        REPEATPMT,
        
        /// <remarks/>
        OTHER,
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
        
        private OFXCREDITCARDMSGSRSV1 cREDITCARDMSGSRSV1Field;
        
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXCREDITCARDMSGSRSV1 CREDITCARDMSGSRSV1 {
            get {
                return this.cREDITCARDMSGSRSV1Field;
            }
            set {
                this.cREDITCARDMSGSRSV1Field = value;
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
        
        private string lANGUAGEField;
        
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
        public string LANGUAGE {
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
        
        private string tRNUIDField;
        
        private STATUS sTATUSField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRS sTMTRSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TRNUID {
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
        
        private CurrencyEnum cURDEFField;
        
        private OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROM bANKACCTFROMField;
        
        private BankTranListType bANKTRANLISTField;
        
        private LedgerBalType lEDGERBALField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CurrencyEnum CURDEF {
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
        public BankTranListType BANKTRANLIST {
            get {
                return this.bANKTRANLISTField;
            }
            set {
                this.bANKTRANLISTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LedgerBalType LEDGERBAL {
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
    public enum CurrencyEnum {
        
        /// <remarks/>
        AED,
        
        /// <remarks/>
        AFA,
        
        /// <remarks/>
        ALL,
        
        /// <remarks/>
        AMD,
        
        /// <remarks/>
        ANG,
        
        /// <remarks/>
        AOA,
        
        /// <remarks/>
        ARS,
        
        /// <remarks/>
        AUD,
        
        /// <remarks/>
        AWG,
        
        /// <remarks/>
        AZM,
        
        /// <remarks/>
        BAM,
        
        /// <remarks/>
        BBD,
        
        /// <remarks/>
        BDT,
        
        /// <remarks/>
        BGL,
        
        /// <remarks/>
        BHD,
        
        /// <remarks/>
        BIF,
        
        /// <remarks/>
        BMD,
        
        /// <remarks/>
        BND,
        
        /// <remarks/>
        BOB,
        
        /// <remarks/>
        BRL,
        
        /// <remarks/>
        BSD,
        
        /// <remarks/>
        BTN,
        
        /// <remarks/>
        BWP,
        
        /// <remarks/>
        BYR,
        
        /// <remarks/>
        BZD,
        
        /// <remarks/>
        CAD,
        
        /// <remarks/>
        CDF,
        
        /// <remarks/>
        CHF,
        
        /// <remarks/>
        CLP,
        
        /// <remarks/>
        CNY,
        
        /// <remarks/>
        COP,
        
        /// <remarks/>
        CRC,
        
        /// <remarks/>
        CUP,
        
        /// <remarks/>
        CVE,
        
        /// <remarks/>
        CYP,
        
        /// <remarks/>
        CZK,
        
        /// <remarks/>
        DJF,
        
        /// <remarks/>
        DKK,
        
        /// <remarks/>
        DOP,
        
        /// <remarks/>
        DZD,
        
        /// <remarks/>
        EEK,
        
        /// <remarks/>
        EGP,
        
        /// <remarks/>
        ERN,
        
        /// <remarks/>
        ETB,
        
        /// <remarks/>
        EUR,
        
        /// <remarks/>
        FJD,
        
        /// <remarks/>
        FKP,
        
        /// <remarks/>
        GBP,
        
        /// <remarks/>
        GEL,
        
        /// <remarks/>
        GGP,
        
        /// <remarks/>
        GHC,
        
        /// <remarks/>
        GIP,
        
        /// <remarks/>
        GMD,
        
        /// <remarks/>
        GNF,
        
        /// <remarks/>
        GTQ,
        
        /// <remarks/>
        GYD,
        
        /// <remarks/>
        HKD,
        
        /// <remarks/>
        HNL,
        
        /// <remarks/>
        HRK,
        
        /// <remarks/>
        HTG,
        
        /// <remarks/>
        HUF,
        
        /// <remarks/>
        IDR,
        
        /// <remarks/>
        ILS,
        
        /// <remarks/>
        IMP,
        
        /// <remarks/>
        INR,
        
        /// <remarks/>
        IQD,
        
        /// <remarks/>
        IRR,
        
        /// <remarks/>
        ISK,
        
        /// <remarks/>
        JEP,
        
        /// <remarks/>
        JMD,
        
        /// <remarks/>
        JOD,
        
        /// <remarks/>
        JPY,
        
        /// <remarks/>
        KES,
        
        /// <remarks/>
        KGS,
        
        /// <remarks/>
        KHR,
        
        /// <remarks/>
        KMF,
        
        /// <remarks/>
        KPW,
        
        /// <remarks/>
        KRW,
        
        /// <remarks/>
        KWD,
        
        /// <remarks/>
        KYD,
        
        /// <remarks/>
        KZT,
        
        /// <remarks/>
        LAK,
        
        /// <remarks/>
        LBP,
        
        /// <remarks/>
        LKR,
        
        /// <remarks/>
        LRD,
        
        /// <remarks/>
        LSL,
        
        /// <remarks/>
        LTL,
        
        /// <remarks/>
        LVL,
        
        /// <remarks/>
        LYD,
        
        /// <remarks/>
        MAD,
        
        /// <remarks/>
        MDL,
        
        /// <remarks/>
        MGF,
        
        /// <remarks/>
        MKD,
        
        /// <remarks/>
        MMK,
        
        /// <remarks/>
        MNT,
        
        /// <remarks/>
        MOP,
        
        /// <remarks/>
        MRO,
        
        /// <remarks/>
        MTL,
        
        /// <remarks/>
        MUR,
        
        /// <remarks/>
        MVR,
        
        /// <remarks/>
        MWK,
        
        /// <remarks/>
        MXN,
        
        /// <remarks/>
        MYR,
        
        /// <remarks/>
        MZM,
        
        /// <remarks/>
        NAD,
        
        /// <remarks/>
        NGN,
        
        /// <remarks/>
        NIO,
        
        /// <remarks/>
        NOK,
        
        /// <remarks/>
        NPR,
        
        /// <remarks/>
        NZD,
        
        /// <remarks/>
        OMR,
        
        /// <remarks/>
        PAB,
        
        /// <remarks/>
        PEN,
        
        /// <remarks/>
        PGK,
        
        /// <remarks/>
        PHP,
        
        /// <remarks/>
        PKR,
        
        /// <remarks/>
        PLN,
        
        /// <remarks/>
        PYG,
        
        /// <remarks/>
        QAR,
        
        /// <remarks/>
        ROL,
        
        /// <remarks/>
        RUR,
        
        /// <remarks/>
        RWF,
        
        /// <remarks/>
        SAR,
        
        /// <remarks/>
        SBD,
        
        /// <remarks/>
        SCR,
        
        /// <remarks/>
        SDD,
        
        /// <remarks/>
        SEK,
        
        /// <remarks/>
        SGD,
        
        /// <remarks/>
        SHP,
        
        /// <remarks/>
        SIT,
        
        /// <remarks/>
        SKK,
        
        /// <remarks/>
        SLL,
        
        /// <remarks/>
        SOS,
        
        /// <remarks/>
        SPL,
        
        /// <remarks/>
        SRG,
        
        /// <remarks/>
        STD,
        
        /// <remarks/>
        SVC,
        
        /// <remarks/>
        SYP,
        
        /// <remarks/>
        SZL,
        
        /// <remarks/>
        THB,
        
        /// <remarks/>
        TJS,
        
        /// <remarks/>
        TMM,
        
        /// <remarks/>
        TND,
        
        /// <remarks/>
        TOP,
        
        /// <remarks/>
        TRL,
        
        /// <remarks/>
        TTD,
        
        /// <remarks/>
        TVD,
        
        /// <remarks/>
        TWD,
        
        /// <remarks/>
        TZS,
        
        /// <remarks/>
        UAH,
        
        /// <remarks/>
        UGX,
        
        /// <remarks/>
        USD,
        
        /// <remarks/>
        UYU,
        
        /// <remarks/>
        UZS,
        
        /// <remarks/>
        VEB,
        
        /// <remarks/>
        VND,
        
        /// <remarks/>
        VUV,
        
        /// <remarks/>
        WST,
        
        /// <remarks/>
        XAF,
        
        /// <remarks/>
        XAG,
        
        /// <remarks/>
        XAU,
        
        /// <remarks/>
        XCD,
        
        /// <remarks/>
        XDR,
        
        /// <remarks/>
        XOF,
        
        /// <remarks/>
        XPD,
        
        /// <remarks/>
        XPF,
        
        /// <remarks/>
        XPT,
        
        /// <remarks/>
        YER,
        
        /// <remarks/>
        YUM,
        
        /// <remarks/>
        ZAR,
        
        /// <remarks/>
        ZMK,
        
        /// <remarks/>
        ZWD,
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
        
        /// <remarks/>
        MONEYMRKT,
        
        /// <remarks/>
        CREDITLINE,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXCREDITCARDMSGSRSV1 {
        
        private OFXCREDITCARDMSGSRSV1CCSTMTTRNRS cCSTMTTRNRSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXCREDITCARDMSGSRSV1CCSTMTTRNRS CCSTMTTRNRS {
            get {
                return this.cCSTMTTRNRSField;
            }
            set {
                this.cCSTMTTRNRSField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXCREDITCARDMSGSRSV1CCSTMTTRNRS {
        
        private string tRNUIDField;
        
        private STATUS sTATUSField;
        
        private OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRS cCSTMTRSField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TRNUID {
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
        public OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRS CCSTMTRS {
            get {
                return this.cCSTMTRSField;
            }
            set {
                this.cCSTMTRSField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRS {
        
        private CurrencyEnum cURDEFField;
        
        private OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRSCCACCTFROM cCACCTFROMField;
        
        private BankTranListType bANKTRANLISTField;
        
        private LedgerBalType lEDGERBALField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CurrencyEnum CURDEF {
            get {
                return this.cURDEFField;
            }
            set {
                this.cURDEFField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRSCCACCTFROM CCACCTFROM {
            get {
                return this.cCACCTFROMField;
            }
            set {
                this.cCACCTFROMField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BankTranListType BANKTRANLIST {
            get {
                return this.bANKTRANLISTField;
            }
            set {
                this.bANKTRANLISTField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LedgerBalType LEDGERBAL {
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRSCCACCTFROM {
        
        private string aCCTIDField;
        
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
    }
}
