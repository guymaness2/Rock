﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rock.NMI {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Scripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Scripts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Rock.NMI.Scripts", typeof(Scripts).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function ($) {
        ///    &apos;use strict&apos;;
        ///    window.Rock = window.Rock || {};
        ///    Rock.NMI = Rock.NMI || {}
        ///    Rock.NMI.controls = Rock.NMI.controls || {};
        ///
        ///    /** JS helper for the gatewayCollectJS */
        ///    Rock.NMI.controls.gatewayCollectJS = (function () {
        ///        var exports = {
        ///            initialize: function (controlId) {
        ///                var self = this;
        ///                var $control = $(&apos;#&apos; + controlId);
        ///
        ///                if ($control.length == 0) {
        ///                    // control hasn&apos;t been re [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string gatewayCollectJS {
            get {
                return ResourceManager.GetString("gatewayCollectJS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sys.Application.add_load(function () {
        ///    /* Threestep gateway related */
        ///    var $step2Submit = $(&apos;.js-step2-submit&apos;);
        ///
        ///    // {{validationGroup}} will get replaced with whatever the validationGroup is
        ///    var validationGroup = &apos;{{validationGroup}}&apos;;
        ///    var $step2Url = $(&apos;.js-step2-url&apos;);
        ///    var $updateProgress = $(&apos;#updateProgress&apos;);
        ///    var $iframeStep2 = $(&apos;.js-step2-iframe&apos;);
        ///    var $addressControl = $(&apos;.js-billingaddress-control&apos;);
        ///
        ///    // {{postbackControlReference}} will get replaced  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string threeStepScript {
            get {
                return ResourceManager.GetString("threeStepScript", resourceCulture);
            }
        }
    }
}
