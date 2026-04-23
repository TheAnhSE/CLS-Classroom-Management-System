import os

OUT = r'd:\Back up D and E\Work\Back up\NET\KI 8 _ Spring 2026\Block 5 _ SP2026\AI\Team Project\CLS-Classroom-Management-System\Document\02_Requirements\Use cases\CLS_UseCaseDiagrams.drawio'
os.makedirs(os.path.dirname(OUT), exist_ok=True)

def xe(s): return s.replace('&','&amp;').replace('"','&quot;').replace('<','&lt;').replace('>','&gt;').replace('\n','&#xa;')

ACTOR = 'shape=mxgraph.uml.actor;whiteSpace=wrap;html=1;fillColor=#ffffff;strokeColor=#000000;labelPosition=center;verticalLabelPosition=bottom;verticalAlign=top;align=center;'
UC    = 'ellipse;whiteSpace=wrap;html=1;fillColor=#ffffff;strokeColor=#000000;fontSize=11;align=center;'
BOX   = 'rounded=0;whiteSpace=wrap;html=1;fillColor=none;strokeColor=#000000;verticalAlign=top;fontSize=12;fontStyle=1;'
ASSOC = 'endArrow=none;html=1;strokeColor=#000000;exitX=1;exitY=0.5;exitDx=0;exitDy=0;'
DASHED= 'endArrow=open;dashed=1;html=1;strokeColor=#000000;endFill=0;fontSize=10;fontStyle=2;'

def G(x,y,w,h): return f'<mxGeometry x="{x}" y="{y}" width="{w}" height="{h}" as="geometry"/>'
def GR(): return '<mxGeometry relative="1" as="geometry"/>'

def actor(i,v,x,y): return f'<mxCell id="{i}" value="{xe(v)}" style="{ACTOR}" vertex="1" parent="1">{G(x,y,40,60)}</mxCell>'
def uc(i,v,x,y,w=150,h=50): return f'<mxCell id="{i}" value="{xe(v)}" style="{UC}" vertex="1" parent="1">{G(x,y,w,h)}</mxCell>'
def box(i,v,x,y,w,h): return f'<mxCell id="{i}" value="{xe(v)}" style="{BOX}" vertex="1" parent="1">{G(x,y,w,h)}</mxCell>'
def assoc(i,s,t): return f'<mxCell id="{i}" value="" style="{ASSOC}" edge="1" source="{s}" target="{t}" parent="1">{GR()}</mxCell>'
def rel(i,s,t,lbl): return f'<mxCell id="{i}" value="{xe(lbl)}" style="{DASHED}" edge="1" source="{s}" target="{t}" parent="1">{GR()}</mxCell>'

def page(pid, name, cells):
    body = '\n'.join(cells)
    return (f'<diagram id="{pid}" name="{xe(name)}">'
            f'<mxGraphModel dx="1422" dy="762" grid="0" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1169" pageHeight="827" math="0" shadow="0">'
            f'<root><mxCell id="0"/><mxCell id="1" parent="0"/>{body}</root>'
            f'</mxGraphModel></diagram>')

# ─── PAGE 1: Academic Admin ────────────────────────────────
p1=[]
p1.append(box('bx1','CLS System',170,30,980,610))
p1.append(actor('ac1','Academic\nAdmin',60,290))

ucs1=[
 ('u01','Login System',           230,80),
 ('u02','Reset Password',         430,80),
 ('u03','Enroll New Learner',     230,175),
 ('u04','View Learner List',      430,175),
 ('u05','Update Learner Profile', 660,175),
 ('u06','Create Learning Package',230,270),
 ('u07','Assign Package\nto Learner',430,270),
 ('u08','Create Session Schedule',230,365),
 ('u09','View Session Timetable', 430,365),
 ('u10','Detect Scheduling\nConflict',660,365),
 ('u11','Record Session\nAttendance',230,460),
 ('u12','View Attendance\nHistory',430,460),
 ('u13','View Admin\nDashboard',  660,460),
 ('u14','Manage User\nAccounts',  860,175),
 ('u15','Manage\nMaster Data',    860,270),
 ('u16','Export Reports',         860,460),
]
for uid,lbl,x,y in ucs1: p1.append(uc(uid,lbl,x,y))

# Associations: Admin → each UC (not system-only)
for uid in ['u01','u03','u04','u05','u06','u07','u08','u09','u11','u12','u13','u14','u15','u16']:
    p1.append(assoc(f'a1_{uid}','ac1',uid))

# Relationships
p1.append(rel('r01','u01','u02','<<extend>>'))
p1.append(rel('r02','u08','u10','<<include>>'))
p1.append(rel('r03','u09','u10','<<include>>'))
p1.append(rel('r04','u06','u07','<<include>>'))

# ─── PAGE 2: Teacher ──────────────────────────────────────
p2=[]
p2.append(box('bx2','CLS System',170,30,780,500))
p2.append(actor('ac2','Teacher',60,260))

ucs2=[
 ('v01','Login System',           230,80),
 ('v02','Reset Password',         450,80),
 ('v03','View Session Timetable', 230,185),
 ('v04','Record Session\nAttendance',450,185),
 ('v05','Submit Session\nFeedback',230,295),
 ('v06','Track SLA\nCompliance (12h)',480,295),
 ('v07','View Feedback\nHistory', 230,395),
 ('v08','Receive SLA\nReminder',  480,395),
]
for uid,lbl,x,y in ucs2: p2.append(uc(uid,lbl,x,y))
for uid in ['v01','v03','v04','v05','v07']:
    p2.append(assoc(f'a2_{uid}','ac2',uid))
p2.append(rel('r21','v01','v02','<<extend>>'))
p2.append(rel('r22','v05','v06','<<include>>'))
p2.append(rel('r23','v05','v08','<<extend>>'))

# ─── PAGE 3: Center Director ──────────────────────────────
p3=[]
p3.append(box('bx3','CLS System',170,30,730,440))
p3.append(actor('ac3','Center\nDirector',60,230))

ucs3=[
 ('w01','Login System',              230,80),
 ('w02','Reset Password',            450,80),
 ('w03','View Director\nDashboard',  230,195),
 ('w04','View Learner List',         450,195),
 ('w05','View Session\nTimetable',   230,310),
 ('w06','Export Operational\nReports',450,310),
]
for uid,lbl,x,y in ucs3: p3.append(uc(uid,lbl,x,y))
for uid in ['w01','w03','w04','w05','w06']:
    p3.append(assoc(f'a3_{uid}','ac3',uid))
p3.append(rel('r31','w01','w02','<<extend>>'))

# ─── PAGE 4: Parent Notification Engine ───────────────────
p4=[]
p4.append(box('bx4','CLS System — Parent Notification Engine',170,30,700,480))
p4.append(actor('ac4s','System\n(Auto)',60,230))
p4.append(actor('ac4e','Email\nService',930,230))
p4.append(actor('ac4p','Parent\n(Sponsor)',930,370))

ucs4=[
 ('n01','Send Attendance\nNotification',   230,80),
 ('n02','Send Schedule Change\nNotification',480,80),
 ('n03','Send Feedback Report\nto Parent', 230,220),
 ('n04','Send Package Expiry\nNotification',480,220),
 ('n05','Receive Package\nExpiry Alert',   230,360),
 ('n06','Dispatch Email\nvia API',         480,360),
]
for uid,lbl,x,y in ucs4: p4.append(uc(uid,lbl,x,y))
for uid in ['n01','n02','n03','n04','n05']:
    p4.append(assoc(f'a4s_{uid}','ac4s',uid))
for uid in ['n01','n02','n03','n04']:
    p4.append(assoc(f'a4e_{uid}',uid,'ac4e'))
p4.append(assoc('a4ep','n01','ac4p'))
p4.append(rel('r41','n01','n06','<<include>>'))
p4.append(rel('r42','n02','n06','<<include>>'))
p4.append(rel('r43','n03','n06','<<include>>'))
p4.append(rel('r44','n04','n06','<<include>>'))

# ─── Build file ───────────────────────────────────────────
xml = ('<?xml version="1.0" encoding="UTF-8"?>\n<mxfile host="app.diagrams.net" version="21.0.0">\n'
       + page('uc1','1.3.2.1 UCs for Academic Admin', p1) + '\n'
       + page('uc2','1.3.2.2 UCs for Teacher',         p2) + '\n'
       + page('uc3','1.3.2.3 UCs for Center Director', p3) + '\n'
       + page('uc4','1.3.2.4 UCs for Parent Notification Engine', p4) + '\n'
       + '</mxfile>')

with open(OUT,'w',encoding='utf-8') as f: f.write(xml)
print('Done. Size:', len(xml))
