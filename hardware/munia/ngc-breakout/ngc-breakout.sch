<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE eagle SYSTEM "eagle.dtd">
<eagle version="7.5.0">
<drawing>
<settings>
<setting alwaysvectorfont="no"/>
<setting verticaltext="up"/>
</settings>
<grid distance="0.1" unitdist="inch" unit="inch" style="lines" multiple="1" display="no" altdistance="0.01" altunitdist="inch" altunit="inch"/>
<layers>
<layer number="1" name="Top" color="4" fill="1" visible="no" active="no"/>
<layer number="2" name="Route2" color="1" fill="3" visible="no" active="no"/>
<layer number="3" name="Route3" color="4" fill="3" visible="no" active="no"/>
<layer number="4" name="Route4" color="1" fill="4" visible="no" active="no"/>
<layer number="5" name="Route5" color="4" fill="4" visible="no" active="no"/>
<layer number="6" name="Route6" color="1" fill="8" visible="no" active="no"/>
<layer number="7" name="Route7" color="4" fill="8" visible="no" active="no"/>
<layer number="8" name="Route8" color="1" fill="2" visible="no" active="no"/>
<layer number="9" name="Route9" color="4" fill="2" visible="no" active="no"/>
<layer number="10" name="Route10" color="1" fill="7" visible="no" active="no"/>
<layer number="11" name="Route11" color="4" fill="7" visible="no" active="no"/>
<layer number="12" name="Route12" color="1" fill="5" visible="no" active="no"/>
<layer number="13" name="Route13" color="4" fill="5" visible="no" active="no"/>
<layer number="14" name="Route14" color="1" fill="6" visible="no" active="no"/>
<layer number="15" name="Route15" color="4" fill="6" visible="no" active="no"/>
<layer number="16" name="Bottom" color="1" fill="1" visible="no" active="no"/>
<layer number="17" name="Pads" color="2" fill="1" visible="no" active="no"/>
<layer number="18" name="Vias" color="2" fill="1" visible="no" active="no"/>
<layer number="19" name="Unrouted" color="6" fill="1" visible="no" active="no"/>
<layer number="20" name="Dimension" color="15" fill="1" visible="no" active="no"/>
<layer number="21" name="tPlace" color="7" fill="1" visible="no" active="no"/>
<layer number="22" name="bPlace" color="7" fill="1" visible="no" active="no"/>
<layer number="23" name="tOrigins" color="15" fill="1" visible="no" active="no"/>
<layer number="24" name="bOrigins" color="15" fill="1" visible="no" active="no"/>
<layer number="25" name="tNames" color="7" fill="1" visible="no" active="no"/>
<layer number="26" name="bNames" color="7" fill="1" visible="no" active="no"/>
<layer number="27" name="tValues" color="7" fill="1" visible="no" active="no"/>
<layer number="28" name="bValues" color="7" fill="1" visible="no" active="no"/>
<layer number="29" name="tStop" color="7" fill="3" visible="no" active="no"/>
<layer number="30" name="bStop" color="7" fill="6" visible="no" active="no"/>
<layer number="31" name="tCream" color="7" fill="4" visible="no" active="no"/>
<layer number="32" name="bCream" color="7" fill="5" visible="no" active="no"/>
<layer number="33" name="tFinish" color="6" fill="3" visible="no" active="no"/>
<layer number="34" name="bFinish" color="6" fill="6" visible="no" active="no"/>
<layer number="35" name="tGlue" color="7" fill="4" visible="no" active="no"/>
<layer number="36" name="bGlue" color="7" fill="5" visible="no" active="no"/>
<layer number="37" name="tTest" color="7" fill="1" visible="no" active="no"/>
<layer number="38" name="bTest" color="7" fill="1" visible="no" active="no"/>
<layer number="39" name="tKeepout" color="4" fill="11" visible="no" active="no"/>
<layer number="40" name="bKeepout" color="1" fill="11" visible="no" active="no"/>
<layer number="41" name="tRestrict" color="4" fill="10" visible="no" active="no"/>
<layer number="42" name="bRestrict" color="1" fill="10" visible="no" active="no"/>
<layer number="43" name="vRestrict" color="2" fill="10" visible="no" active="no"/>
<layer number="44" name="Drills" color="7" fill="1" visible="no" active="no"/>
<layer number="45" name="Holes" color="7" fill="1" visible="no" active="no"/>
<layer number="46" name="Milling" color="3" fill="1" visible="no" active="no"/>
<layer number="47" name="Measures" color="7" fill="1" visible="no" active="no"/>
<layer number="48" name="Document" color="7" fill="1" visible="no" active="no"/>
<layer number="49" name="Reference" color="7" fill="1" visible="no" active="no"/>
<layer number="51" name="tDocu" color="7" fill="1" visible="no" active="no"/>
<layer number="52" name="bDocu" color="7" fill="1" visible="no" active="no"/>
<layer number="90" name="Modules" color="5" fill="1" visible="yes" active="yes"/>
<layer number="91" name="Nets" color="2" fill="1" visible="yes" active="yes"/>
<layer number="92" name="Busses" color="1" fill="1" visible="yes" active="yes"/>
<layer number="93" name="Pins" color="2" fill="1" visible="yes" active="yes"/>
<layer number="94" name="Symbols" color="4" fill="1" visible="yes" active="yes"/>
<layer number="95" name="Names" color="7" fill="1" visible="yes" active="yes"/>
<layer number="96" name="Values" color="7" fill="1" visible="yes" active="yes"/>
<layer number="97" name="Info" color="7" fill="1" visible="yes" active="yes"/>
<layer number="98" name="Guide" color="6" fill="1" visible="yes" active="yes"/>
<layer number="99" name="SpiceOrder" color="7" fill="1" visible="yes" active="yes"/>
</layers>
<schematic xreflabel="%F%N/%S.%C%R" xrefpart="/%S.%C%R">
<libraries>
<library name="wirepad">
<description>&lt;b&gt;Single Pads&lt;/b&gt;&lt;p&gt;
&lt;author&gt;Created by librarian@cadsoft.de&lt;/author&gt;</description>
<packages>
<package name="SMD1,27-2,54">
<description>&lt;b&gt;SMD PAD&lt;/b&gt;</description>
<smd name="1" x="0" y="0" dx="1.27" dy="2.54" layer="1"/>
<text x="0" y="0" size="0.0254" layer="27">&gt;VALUE</text>
<text x="-0.8" y="-2.4" size="1.27" layer="25" rot="R90">&gt;NAME</text>
</package>
</packages>
<symbols>
<symbol name="PAD">
<wire x1="-1.016" y1="1.016" x2="1.016" y2="-1.016" width="0.254" layer="94"/>
<wire x1="-1.016" y1="-1.016" x2="1.016" y2="1.016" width="0.254" layer="94"/>
<text x="-1.143" y="1.8542" size="1.778" layer="95">&gt;NAME</text>
<text x="-1.143" y="-3.302" size="1.778" layer="96">&gt;VALUE</text>
<pin name="P" x="2.54" y="0" visible="off" length="short" direction="pas" rot="R180"/>
</symbol>
</symbols>
<devicesets>
<deviceset name="SMD2" prefix="PAD" uservalue="yes">
<description>&lt;b&gt;SMD PAD&lt;/b&gt;</description>
<gates>
<gate name="1" symbol="PAD" x="0" y="0"/>
</gates>
<devices>
<device name="" package="SMD1,27-2,54">
<connects>
<connect gate="1" pin="P" pad="1"/>
</connects>
<technologies>
<technology name=""/>
</technologies>
</device>
</devices>
</deviceset>
</devicesets>
</library>
<library name="supply1">
<description>&lt;b&gt;Supply Symbols&lt;/b&gt;&lt;p&gt;
 GND, VCC, 0V, +5V, -5V, etc.&lt;p&gt;
 Please keep in mind, that these devices are necessary for the
 automatic wiring of the supply signals.&lt;p&gt;
 The pin name defined in the symbol is identical to the net which is to be wired automatically.&lt;p&gt;
 In this library the device names are the same as the pin names of the symbols, therefore the correct signal names appear next to the supply symbols in the schematic.&lt;p&gt;
 &lt;author&gt;Created by librarian@cadsoft.de&lt;/author&gt;</description>
<packages>
</packages>
<symbols>
<symbol name="GND">
<wire x1="-1.905" y1="0" x2="1.905" y2="0" width="0.254" layer="94"/>
<text x="-2.54" y="-2.54" size="1.778" layer="96">&gt;VALUE</text>
<pin name="GND" x="0" y="2.54" visible="off" length="short" direction="sup" rot="R270"/>
</symbol>
<symbol name="+5V">
<wire x1="1.27" y1="-1.905" x2="0" y2="0" width="0.254" layer="94"/>
<wire x1="0" y1="0" x2="-1.27" y2="-1.905" width="0.254" layer="94"/>
<text x="-2.54" y="-5.08" size="1.778" layer="96" rot="R90">&gt;VALUE</text>
<pin name="+5V" x="0" y="-2.54" visible="off" length="short" direction="sup" rot="R90"/>
</symbol>
</symbols>
<devicesets>
<deviceset name="GND" prefix="GND">
<description>&lt;b&gt;SUPPLY SYMBOL&lt;/b&gt;</description>
<gates>
<gate name="1" symbol="GND" x="0" y="0"/>
</gates>
<devices>
<device name="">
<technologies>
<technology name=""/>
</technologies>
</device>
</devices>
</deviceset>
<deviceset name="+5V" prefix="P+">
<description>&lt;b&gt;SUPPLY SYMBOL&lt;/b&gt;</description>
<gates>
<gate name="1" symbol="+5V" x="0" y="0"/>
</gates>
<devices>
<device name="">
<technologies>
<technology name=""/>
</technologies>
</device>
</devices>
</deviceset>
</devicesets>
</library>
<library name="munia-breakouts">
<description>Generated from &lt;b&gt;breakout.sch&lt;/b&gt;&lt;p&gt;
by exp-lbrs.ulp</description>
<packages>
<package name="NGC-CONNECTOR">
<wire x1="8.05" y1="-6.135" x2="-8.05" y2="-6.135" width="0.127" layer="21"/>
<wire x1="8.05" y1="5.865" x2="-8.05" y2="5.865" width="0.127" layer="21"/>
<wire x1="8.05" y1="5.865" x2="8.05" y2="-6.135" width="0.127" layer="21"/>
<wire x1="-8.05" y1="-6.135" x2="-8.05" y2="5.865" width="0.127" layer="21"/>
<pad name="3" x="1.905" y="0.635" drill="0.65" rot="R90"/>
<pad name="6" x="1.905" y="-0.635" drill="0.65" rot="R90"/>
<pad name="2" x="0" y="0.635" drill="0.65" rot="R90"/>
<pad name="1" x="-1.905" y="0.635" drill="0.65" rot="R90"/>
<pad name="4" x="-1.905" y="-0.635" drill="0.65" rot="R90"/>
<pad name="5" x="0" y="-0.635" drill="0.65" rot="R270"/>
<smd name="7" x="0" y="5.115" dx="2.1" dy="0.6" layer="16"/>
<text x="0" y="1.905" size="0.6096" layer="22" font="vector" ratio="14" rot="SMR270" align="center-right">+5V

DAT

GND</text>
<text x="0" y="-1.905" size="0.6096" layer="22" font="vector" ratio="14" rot="SMR270" align="center-left">MGND



3.3V</text>
</package>
</packages>
<symbols>
<symbol name="NGC-CONNECTOR">
<wire x1="-3.81" y1="-5.08" x2="3.81" y2="-5.08" width="0.4064" layer="94"/>
<wire x1="3.81" y1="-5.08" x2="3.81" y2="7.62" width="0.4064" layer="94"/>
<wire x1="3.81" y1="7.62" x2="-3.81" y2="7.62" width="0.4064" layer="94"/>
<wire x1="-3.81" y1="7.62" x2="-3.81" y2="-5.08" width="0.4064" layer="94"/>
<pin name="5V" x="0" y="5.08" length="short" direction="pas" function="dot"/>
<pin name="DATA" x="0" y="2.54" length="short" direction="pas" function="dot"/>
<pin name="GND" x="0" y="-2.54" length="short" direction="pas" function="dot"/>
<pin name="3V3" x="0" y="0" length="short" direction="pas" function="dot"/>
<text x="-3.81" y="8.255" size="1.778" layer="95">&gt;NAME</text>
</symbol>
</symbols>
<devicesets>
<deviceset name="NGC-CONNECTOR">
<gates>
<gate name="G$1" symbol="NGC-CONNECTOR" x="0" y="0"/>
</gates>
<devices>
<device name="GC-CONN2" package="NGC-CONNECTOR">
<connects>
<connect gate="G$1" pin="3V3" pad="6"/>
<connect gate="G$1" pin="5V" pad="1"/>
<connect gate="G$1" pin="DATA" pad="2"/>
<connect gate="G$1" pin="GND" pad="3 4 7"/>
</connects>
<technologies>
<technology name=""/>
</technologies>
</device>
</devices>
</deviceset>
</devicesets>
</library>
</libraries>
<attributes>
</attributes>
<variantdefs>
</variantdefs>
<classes>
<class number="0" name="default" width="0" drill="0">
</class>
</classes>
<parts>
<part name="PAD_GND" library="wirepad" deviceset="SMD2" device=""/>
<part name="GND1" library="supply1" deviceset="GND" device=""/>
<part name="U$1" library="munia-breakouts" deviceset="NGC-CONNECTOR" device="GC-CONN2"/>
<part name="PAD_DATA" library="wirepad" deviceset="SMD2" device=""/>
<part name="PAD_5V" library="wirepad" deviceset="SMD2" device=""/>
<part name="P+1" library="supply1" deviceset="+5V" device=""/>
<part name="PAD_GND1" library="wirepad" deviceset="SMD2" device=""/>
<part name="PAD_DATA1" library="wirepad" deviceset="SMD2" device=""/>
<part name="PAD_5V1" library="wirepad" deviceset="SMD2" device=""/>
</parts>
<sheets>
<sheet>
<plain>
</plain>
<instances>
<instance part="PAD_GND" gate="1" x="35.56" y="73.66" rot="MR0"/>
<instance part="GND1" gate="1" x="27.94" y="68.58"/>
<instance part="U$1" gate="G$1" x="10.16" y="76.2" rot="MR0"/>
<instance part="PAD_DATA" gate="1" x="35.56" y="83.82" rot="MR0"/>
<instance part="PAD_5V" gate="1" x="35.56" y="93.98" rot="MR0"/>
<instance part="P+1" gate="1" x="22.86" y="101.6"/>
<instance part="PAD_GND1" gate="1" x="50.8" y="73.66" rot="MR0"/>
<instance part="PAD_DATA1" gate="1" x="50.8" y="83.82" rot="MR0"/>
<instance part="PAD_5V1" gate="1" x="50.8" y="93.98" rot="MR0"/>
</instances>
<busses>
</busses>
<nets>
<net name="GND" class="0">
<segment>
<pinref part="PAD_GND" gate="1" pin="P"/>
<wire x1="33.02" y1="73.66" x2="27.94" y2="73.66" width="0.1524" layer="91"/>
<pinref part="GND1" gate="1" pin="GND"/>
<wire x1="27.94" y1="71.12" x2="27.94" y2="73.66" width="0.1524" layer="91"/>
<pinref part="U$1" gate="G$1" pin="GND"/>
<wire x1="10.16" y1="73.66" x2="27.94" y2="73.66" width="0.1524" layer="91"/>
<junction x="27.94" y="73.66"/>
<pinref part="PAD_GND1" gate="1" pin="P"/>
<wire x1="33.02" y1="73.66" x2="48.26" y2="73.66" width="0.1524" layer="91"/>
<junction x="33.02" y="73.66"/>
</segment>
</net>
<net name="N$1" class="0">
<segment>
<pinref part="U$1" gate="G$1" pin="DATA"/>
<wire x1="10.16" y1="78.74" x2="25.4" y2="78.74" width="0.1524" layer="91"/>
<wire x1="25.4" y1="78.74" x2="25.4" y2="83.82" width="0.1524" layer="91"/>
<wire x1="25.4" y1="83.82" x2="33.02" y2="83.82" width="0.1524" layer="91"/>
<pinref part="PAD_DATA" gate="1" pin="P"/>
<pinref part="PAD_DATA1" gate="1" pin="P"/>
<wire x1="33.02" y1="83.82" x2="48.26" y2="83.82" width="0.1524" layer="91"/>
<junction x="33.02" y="83.82"/>
</segment>
</net>
<net name="+5V" class="0">
<segment>
<pinref part="U$1" gate="G$1" pin="5V"/>
<wire x1="10.16" y1="81.28" x2="22.86" y2="81.28" width="0.1524" layer="91"/>
<wire x1="22.86" y1="81.28" x2="22.86" y2="93.98" width="0.1524" layer="91"/>
<wire x1="22.86" y1="93.98" x2="33.02" y2="93.98" width="0.1524" layer="91"/>
<pinref part="PAD_5V" gate="1" pin="P"/>
<pinref part="P+1" gate="1" pin="+5V"/>
<wire x1="22.86" y1="99.06" x2="22.86" y2="93.98" width="0.1524" layer="91"/>
<junction x="22.86" y="93.98"/>
<wire x1="33.02" y1="93.98" x2="48.26" y2="93.98" width="0.1524" layer="91"/>
<pinref part="PAD_5V1" gate="1" pin="P"/>
<junction x="33.02" y="93.98"/>
</segment>
</net>
</nets>
</sheet>
</sheets>
</schematic>
</drawing>
<compatibility>
<note version="6.3" minversion="6.2.2" severity="warning">
Since Version 6.2.2 text objects can contain more than one line,
which will not be processed correctly with this version.
</note>
</compatibility>
</eagle>
