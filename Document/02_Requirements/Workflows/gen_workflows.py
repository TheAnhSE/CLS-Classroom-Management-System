import os

OUT = r'd:\Back up D and E\Work\Back up\NET\KI 8 _ Spring 2026\Block 5 _ SP2026\AI\Team Project\CLS-Classroom-Management-System\Document\02_Requirements\Workflows\CLS_Main_Workflows.drawio'

def xe(s):
    return s.replace('&','&amp;').replace('"','&quot;').replace('<','&lt;').replace('>','&gt;').replace('\n','&#xa;')

# Styles matching template
POOL  = 'shape=pool;startSize=0;horizontal=1;fillColor=#dae8fc;strokeColor=#6c8ebf;fontStyle=0;'
LANE  = 'swimlane;startSize=30;horizontal=0;fillColor=#666666;fontColor=#ffffff;strokeColor=#b0b0b0;fontSize=11;fontStyle=1;swimlaneHead=0;'
TG    = 'rounded=1;whiteSpace=wrap;html=1;fillColor=#d5e8d4;strokeColor=#82b366;fontSize=11;'   # green task
TP    = 'rounded=1;whiteSpace=wrap;html=1;fillColor=#f8cecc;strokeColor=#b85450;fontSize=11;'   # pink task
TB    = 'rounded=1;whiteSpace=wrap;html=1;fillColor=#dae8fc;strokeColor=#6c8ebf;fontSize=11;'   # blue task
TY    = 'rounded=1;whiteSpace=wrap;html=1;fillColor=#fff2cc;strokeColor=#d6b656;fontSize=11;'   # yellow task
DEC   = 'rhombus;whiteSpace=wrap;html=1;fillColor=#ffd966;strokeColor=#d6a200;fontSize=10;'
START = 'ellipse;aspect=fixed;fillColor=#000000;strokeColor=#000000;'
END   = 'ellipse;aspect=fixed;fillColor=#000000;strokeColor=#000000;strokeWidth=3;'
EDGE  = 'edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=0;entryY=0.5;entryDx=0;entryDy=0;'

def G(x,y,w,h): return f'<mxGeometry x="{x}" y="{y}" width="{w}" height="{h}" as="geometry"/>'
def Gr(): return '<mxGeometry relative="1" as="geometry"/>'

def pool(i,v,x,y,w,h):  return f'<mxCell id="{i}" value="{xe(v)}" style="{POOL}" vertex="1" parent="1">{G(x,y,w,h)}</mxCell>'
def lane(i,v,p,y,w,h):  return f'<mxCell id="{i}" value="{xe(v)}" style="{LANE}" vertex="1" parent="{p}"><mxGeometry x="0" y="{y}" width="{w}" height="{h}" as="geometry"/></mxCell>'
def task(i,v,p,x,y,w,h,st=TG): return f'<mxCell id="{i}" value="{xe(v)}" style="{st}" vertex="1" parent="{p}">{G(x,y,w,h)}</mxCell>'
def dec(i,v,p,x,y,w=120,h=70): return f'<mxCell id="{i}" value="{xe(v)}" style="{DEC}" vertex="1" parent="{p}">{G(x,y,w,h)}</mxCell>'
def stt(i,p,x,y,st=START): return f'<mxCell id="{i}" value="" style="{st}" vertex="1" parent="{p}">{G(x,y,24,24)}</mxCell>'
def edge(i,s,t,lbl='',p='1'): return f'<mxCell id="{i}" value="{xe(lbl)}" style="{EDGE}" edge="1" source="{s}" target="{t}" parent="{p}">{Gr()}</mxCell>'

def diagram(did, name, cells):
    body = '\n'.join(cells)
    return (f'<diagram id="{did}" name="{xe(name)}">'
            f'<mxGraphModel dx="1422" dy="762" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1654" pageHeight="1169" math="0" shadow="0">'
            f'<root><mxCell id="0"/><mxCell id="1" parent="0"/>'
            f'{body}</root></mxGraphModel></diagram>')

W=1560; H=150

# ── WF1: Learner Enrollment ──────────────────────────────────────────
c1=[]
c1.append(pool('P1','1.2.1  Learner Enrollment & Learning Package Assignment  (UC-03, UC-07, UC-09)',40,20,W,H*3+60))
c1.append(lane('LA','ACADEMIC ADMIN','P1',  0,W,H))
c1.append(lane('LS','SYSTEM',        'P1',  H,W,H))
c1.append(lane('LE','EMAIL SERVICE', 'P1',H*2,W,H))

c1.append(stt('a0','LA',20,63))
c1.append(task('a1','Select\n"Add New Learner"','LA', 60,55,130,50,TP))
c1.append(task('a2','Enter Learner Info\n& Parent Contact',  'LA',230,55,150,50,TB))
c1.append(task('a3','Select Learning\nPackage for Learner', 'LA',970,55,150,50,TB))
c1.append(task('a4','Confirm & Save\nEnrollment',           'LA',1200,55,140,50,TG))
c1.append(stt('a5','LA',1410,63,END))

c1.append(task('s1','Validate Data\nCheck Parent Email (BR-LRN-01)','LS',230,55,160,50,TG))
c1.append(dec( 'd1','Parent Email\nalready exists?',               'LS',430,40))
c1.append(task('s2','Link Learner to\nexisting Parent ID',         'LS',600,10,150,50,TG))
c1.append(task('s3','Create new\nParent record in DB',             'LS',600,90,150,50,TG))
c1.append(task('s4','Create Learner record\n(Status: Active)',     'LS',800,55,150,50,TG))
c1.append(task('s5','Initialize\nRemainingSessions (BR-PKG-03)',   'LS',970,55,160,50,TG))
c1.append(task('s6','Trigger email\nNotification to Parent',       'LS',1200,55,150,50,TG))

c1.append(task('e1','Send confirmation\nemail to Parent',          'LE',1200,55,155,50,TY))

for i,s,t in [('e_a01','a0','a1'),('e_a12','a1','a2'),('e_a2s1','a2','s1'),
               ('e_s1d1','s1','d1'),('e_s2s4','s2','s4'),('e_s3s4','s3','s4'),
               ('e_s4a3','s4','a3'),('e_a3s5','a3','s5'),('e_s5a4','s5','a4'),
               ('e_a4s6','a4','s6'),('e_s6e1','s6','e1'),('e_a4a5','a4','a5')]:
    c1.append(edge(i,s,t))
c1.append(edge('e_d1s2','d1','s2','Yes'))
c1.append(edge('e_d1s3','d1','s3','No'))

# ── WF2: Session Scheduling ──────────────────────────────────────────
c2=[]
c2.append(pool('P2','1.2.2  Session Scheduling & Conflict Detection  (UC-10, UC-11, UC-13)',40,20,W,H*2+40))
c2.append(lane('LA2','ACADEMIC ADMIN','P2', 0,W,H))
c2.append(lane('LS2','SYSTEM',        'P2', H,W,H))

c2.append(stt('b0','LA2',20,63))
c2.append(task('b1','Open\n"Create Session" Form',         'LA2', 60,55,130,50,TP))
c2.append(task('b2','Select: Subject, Date\nTime, Room, Teacher','LA2',230,55,155,50,TB))
c2.append(task('b3','Add Learner list\nto Session',        'LA2',430,55,140,50,TB))
c2.append(task('b4','Review & Correct\nSession Details',   'LA2',760,55,145,50,TB))
c2.append(task('b5','Confirm &\nSave Schedule',            'LA2',1030,55,140,50,TG))
c2.append(stt('b6','LA2',1360,63,END))

c2.append(task('s10','Run Conflict Detection\nAlgorithm (BR-SCH-01)',  'LS2',430,55,155,50,TG))
c2.append(dec( 'd2','Scheduling\nConflict?',                           'LS2',635,40))
c2.append(task('s11','Show Error:\nMSG-SCH-409 / MSG-SCH-410',        'LS2',810,10,165,50,TP))
c2.append(task('s12','Create Session record\nin DB',                   'LS2',810,90,165,50,TG))
c2.append(task('s13','Trigger Schedule Change\nNotification (UC-20)',  'LS2',1030,55,165,50,TY))

for i,s,t in [('eb01','b0','b1'),('eb12','b1','b2'),('eb23','b2','b3'),
               ('eb3s10','b3','s10'),('es10d2','s10','d2'),
               ('es11b4','s11','b4'),('eb4b3','b4','b3'),
               ('es12b5','s12','b5'),('eb5s13','b5','s13'),('eb56','b5','b6')]:
    c2.append(edge(i,s,t))
c2.append(edge('ed2s11','d2','s11','Yes'))
c2.append(edge('ed2s12','d2','s12','No'))

# ── WF3: Attendance & Notification ───────────────────────────────────
c3=[]
c3.append(pool('P3','1.2.3  Attendance Recording & Parent Notification  (UC-14, UC-16, UC-19)',40,20,W,H*3+60))
c3.append(lane('LT3','TEACHER / ADMIN','P3',  0,W,H))
c3.append(lane('LS3','SYSTEM',         'P3',  H,W,H))
c3.append(lane('LP3','PARENT (EMAIL)', 'P3',H*2,W,H))

c3.append(stt('t0','LT3',20,63))
c3.append(task('t1','Open Attendance Form\nfor Today\'s Session',   'LT3', 60,55,150,50,TP))
c3.append(task('t2','Mark each Learner:\nPresent / Absent / Late', 'LT3',255,55,150,50,TB))
c3.append(task('t3','Submit Attendance',                           'LT3',450,55,130,50,TB))
c3.append(task('t4','View saved\nAttendance Result',               'LT3',1190,55,150,50,TG))
c3.append(stt('t5','LT3',1400,63,END))

c3.append(task('s20','Deduct 1 session from\nLearnerPackage (BR-ATT-01)', 'LS3',450,10,165,50,TG))
c3.append(dec( 'd3','Remaining sessions\n<= 2-week threshold?',           'LS3',665,5,140,70))
c3.append(task('s21','Create in-app Alert\nfor Academic Admin',            'LS3',860,10,155,50,TP))
c3.append(task('s22','Log Attendance\nrecord in DB',                      'LS3',450,90,165,50,TG))
c3.append(task('s23','Compose Email:\nAttendance Notification',           'LS3',665,90,155,50,TG))
c3.append(task('s24','Send request\nto Email Service API',                'LS3',870,90,155,50,TG))
c3.append(task('s25','Log Notification\n(Status: Sent / Failed)',         'LS3',1075,55,155,50,TG))

c3.append(task('p1','Receive Email:\nLearner Attendance Report',          'LP3',870,55,160,50,TY))

for i,s,t in [('et01','t0','t1'),('et12','t1','t2'),('et23','t2','t3'),
               ('et3s20','t3','s20'),('es20d3','s20','d3'),('ed3s21','d3','s21'),
               ('et3s22','t3','s22'),('es22s23','s22','s23'),('es23s24','s23','s24'),
               ('es24p1','s24','p1'),('es24s25','s24','s25'),
               ('es25t4','s25','t4'),('et45','t4','t5')]:
    c3.append(edge(i,s,t))
c3.append(edge('ed3s21b','d3','s21','Yes'))
c3.append(edge('ed3go','d3','s23','No'))

# ── WF4: Teacher Feedback & SLA ──────────────────────────────────────
c4=[]
c4.append(pool('P4','1.2.4  Teacher Feedback Submission & 12-hour SLA Tracking  (UC-16, UC-18, UC-21)',40,20,W,H*3+60))
c4.append(lane('LF4','TEACHER',        'P4',  0,W,H))
c4.append(lane('LS4','SYSTEM',         'P4',  H,W,H))
c4.append(lane('LP4','PARENT (EMAIL)', 'P4',H*2,W,H))

c4.append(stt('f0','LF4',20,63))
c4.append(task('f1','Session ends',                           'LF4', 60,55,120,50,TP))
c4.append(task('f2','Select Learner\nto write Feedback',      'LF4',310,55,145,50,TB))
c4.append(task('f3','Enter Rating (1-5)\n& Academic Notes',   'LF4',500,55,155,50,TB))
c4.append(task('f4','Submit Feedback',                        'LF4',700,55,130,50,TB))
c4.append(task('f5','View submitted\nFeedback record',        'LF4',1220,55,150,50,TG))
c4.append(stt('f6','LF4',1420,63,END))

c4.append(task('fs1','Start 12h SLA Timer\n(Session end time)',          'LS4', 60,55,180,50,TG))
c4.append(task('fs2','Check: Attendance\nrecorded? (BR-FDB-01)',         'LS4',500,55,165,50,TG))
c4.append(dec( 'fd1','Attendance\nrecorded?',                            'LS4',715,40))
c4.append(task('fs3','Block Submit:\nMSG-FDB-401',                       'LS4',890,10,150,50,TP))
c4.append(task('fs4','Calculate SLA:\nDelta = Now - SessionEndTime',     'LS4',890,85,175,50,TG))
c4.append(dec( 'fd2','Delta\n<= 12 hours?',                             'LS4',1120,40))
c4.append(task('fs5','IsSLACompliant = True\nSave Feedback to DB',       'LS4',1300,10,175,50,TG))
c4.append(task('fs6','IsSLACompliant = False\nFlag as Late — Alert Admin','LS4',1300,85,175,50,TP))

c4.append(task('fp1','Receive Email:\nSession Feedback Report',          'LP4',1300,55,170,50,TY))

for i,s,t in [('ef01','f0','f1'),('ef12','f1','f2'),('ef23','f2','f3'),
               ('ef34','f3','f4'),('ef4fs2','f4','fs2'),('efs2fd1','fs2','fd1'),
               ('efs3f2','fs3','f2'),('efs4fd2','fs4','fd2'),
               ('efs5fp1','fs5','fp1'),('efs6fp1','fs6','fp1'),
               ('efs5f5','fs5','f5'),('ef56','f5','f6')]:
    c4.append(edge(i,s,t))
c4.append(edge('ef1fs1','f1','fs1'))
c4.append(edge('efd1n','fd1','fs3','No'))
c4.append(edge('efd1y','fd1','fs4','Yes'))
c4.append(edge('efd2y','fd2','fs5','Yes'))
c4.append(edge('efd2n','fd2','fs6','No'))

xml = ('<?xml version="1.0" encoding="UTF-8"?>\n<mxfile host="app.diagrams.net" version="21.0.0">\n'
       + diagram('wf1','1.2.1 Learner Enrollment',c1) + '\n'
       + diagram('wf2','1.2.2 Session Scheduling',c2) + '\n'
       + diagram('wf3','1.2.3 Attendance & Notification',c3) + '\n'
       + diagram('wf4','1.2.4 Feedback & SLA Tracking',c4) + '\n'
       + '</mxfile>')

with open(OUT,'w',encoding='utf-8') as f: f.write(xml)
print('Done. Size:', len(xml))
