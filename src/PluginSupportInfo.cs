////////////////////////////////////////////////////////////////////////
//
// This file is part of gmic-sharp-pdn-example, an example Paint.NET
// Effect plugin that uses the gmic-sharp-pdn library.
//
// Copyright (c) 2020 Nicholas Hayes
//
// This file is licensed under the MIT License.
// See LICENSE.txt for complete licensing and attribution information.
//
////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using System;
using System.Reflection;

namespace GmicSharpPdnExample
{
    public class PluginSupportInfo : IPluginSupportInfo
    {
        private readonly Assembly assembly = typeof(PluginSupportInfo).Assembly;

        public string Author => "Nicholas Hayes and David Tschumperlé";

        public string Copyright => this.assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;

        public string DisplayName => this.assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        public Version Version => this.assembly.GetName().Version;

        public Uri WebsiteUri => new Uri("https://forums.getpaint.net/topic/116417-gmicsharppdn");
    }
}
