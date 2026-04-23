import os

OUT_DIR = r'd:\Back up D and E\Work\Back up\NET\KI 8 _ Spring 2026\Block 5 _ SP2026\AI\Team Project\CLS-Classroom-Management-System\Document\02_Requirements\ERD'
os.makedirs(OUT_DIR, exist_ok=True)
OUT = OUT_DIR + r'\CLS_ERD.drawio'

def xe(s):
    return s.replace('&','&amp;').replace('"','&quot;').replace('<','&lt;').replace('>','&gt;').replace('\n','&#xa;')

HDR  = 'swimlane;fontStyle=1;align=center;startSize=26;fillColor=#dae8fc;strokeColor=#6c8ebf;fontSize=12;horizontal=1;'
PK   = 'text;strokeColor=none;fillColor=none;align=left;verticalAlign=middle;spacingLeft=6;overflow=hidden;rotatable=0;fontSize=11;fontStyle=4;points=[[0,0.5],[1,0.5]];portConstraint=eastwest;'  # underline
ATT  = 'text;strokeColor=none;fillColor=none;align=left;verticalAlign=middle;spacingLeft=6;overflow=hidden;rotatable=0;fontSize=11;fontStyle=0;points=[[0,0.5],[1,0.5]];portConstraint=eastwest;'
FK   = 'text;strokeColor=none;fillColor=none;align=left;verticalAlign=middle;spacingLeft=6;overflow=hidden;rotatable=0;fontSize=11;fontStyle=2;points=[[0,0.5],[1,0.5]];portConstraint=eastwest;'  # italic
EDGE = 'edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;startFill=0;endFill=0;fontSize=10;'

cells = []
RH = 22  # row height

def G(x,y,w,h): return f'<mxGeometry x="{x}" y="{y}" width="{w}" height="{h}" as="geometry"/>'
def GR():       return '<mxGeometry relative="1" as="geometry"/>'

def entity(eid, name, x, y, w, attrs):
    """attrs = list of (label, kind) where kind='pk'|'fk'|'att'"""
    h = 26 + len(attrs) * RH
    cells.append(f'<mxCell id="{eid}" value="{xe(name)}" style="{HDR}" vertex="1" parent="1">{G(x,y,w,h)}</mxCell>')
    for i,(lbl,kind) in enumerate(attrs):
        st = PK if kind=='pk' else (FK if kind=='fk' else ATT)
        cells.append(f'<mxCell id="{eid}_r{i}" value="{xe(lbl)}" style="{st}" vertex="1" parent="{eid}">{G(0,26+i*RH,w,RH)}</mxCell>')

def edge(eid, src, tgt, lbl, sa, ea):
    st = f'{EDGE}startArrow={sa};endArrow={ea};'
    cells.append(f'<mxCell id="{eid}" value="{xe(lbl)}" style="{st}" edge="1" source="{src}" target="{tgt}" parent="1">{GR()}</mxCell>')

# ── ENTITIES ─────────────────────────────────────────────────────────
# Crow-foot: ERmandOne | ERone | ERmany | ERzeroToMany | ERoneToMany

# Row 1
entity('E_roles','roles',30,30,180,[
    ('+ role_id (PK)','pk'),
    ('+ role_name','att'),
    ('+ description','att'),
    ('+ is_active','att'),
])
entity('E_users','users',270,30,210,[
    ('+ user_id (PK)','pk'),
    ('+ role_id (FK)','fk'),
    ('+ email','att'),
    ('+ first_name / last_name','att'),
    ('+ phone_number','att'),
    ('+ is_active','att'),
    ('+ created_time','att'),
])
entity('E_activity','activity_log',560,30,200,[
    ('+ log_id (PK)','pk'),
    ('+ user_id (FK)','fk'),
    ('+ action','att'),
    ('+ entity_type','att'),
    ('+ entity_id','att'),
    ('+ ip_address','att'),
    ('+ created_time','att'),
])
entity('E_subjects','subjects',870,30,190,[
    ('+ subject_id (PK)','pk'),
    ('+ subject_name','att'),
    ('+ description','att'),
    ('+ is_active','att'),
])
entity('E_classrooms','classrooms',1130,30,190,[
    ('+ classroom_id (PK)','pk'),
    ('+ room_name','att'),
    ('+ capacity','att'),
    ('+ location','att'),
    ('+ is_active','att'),
])

# Row 2
entity('E_parents','parents',30,310,190,[
    ('+ parent_id (PK)','pk'),
    ('+ full_name','att'),
    ('+ email','att'),
    ('+ phone_number','att'),
    ('+ address','att'),
    ('+ relationship','att'),
])
entity('E_learners','learners',280,310,200,[
    ('+ learner_id (PK)','pk'),
    ('+ parent_id (FK)','fk'),
    ('+ first_name / last_name','att'),
    ('+ date_of_birth','att'),
    ('+ gender','att'),
    ('+ enrollment_date','att'),
    ('+ status','att'),
])
entity('E_lpkg','learner_packages',550,310,205,[
    ('+ learner_package_id (PK)','pk'),
    ('+ learner_id (FK)','fk'),
    ('+ package_id (FK)','fk'),
    ('+ assigned_date','att'),
    ('+ expiry_date','att'),
    ('+ total_sessions','att'),
    ('+ remaining_sessions','att'),
    ('+ status','att'),
])
entity('E_packages','learning_packages',830,310,200,[
    ('+ package_id (PK)','pk'),
    ('+ subject_id (FK)','fk'),
    ('+ package_name','att'),
    ('+ total_sessions','att'),
    ('+ duration_months','att'),
    ('+ tuition_fee','att'),
    ('+ is_active','att'),
])

# Row 3
entity('E_notif','notifications',30,600,210,[
    ('+ notification_id (PK)','pk'),
    ('+ parent_id (FK)','fk'),
    ('+ learner_id (FK)','fk'),
    ('+ type','att'),
    ('+ recipient_email','att'),
    ('+ subject','att'),
    ('+ status','att'),
    ('+ sent_time','att'),
])
entity('E_sl','session_learners',310,600,200,[
    ('+ session_learner_id (PK)','pk'),
    ('+ session_id (FK)','fk'),
    ('+ learner_id (FK)','fk'),
])
entity('E_attend','attendance',580,600,205,[
    ('+ attendance_id (PK)','pk'),
    ('+ session_id (FK)','fk'),
    ('+ learner_id (FK)','fk'),
    ('+ status','att'),
    ('+ notes','att'),
    ('+ recorded_by (FK)','fk'),
    ('+ recorded_time','att'),
])
entity('E_sessions','sessions',860,600,205,[
    ('+ session_id (PK)','pk'),
    ('+ subject_id (FK)','fk'),
    ('+ classroom_id (FK)','fk'),
    ('+ teacher_id (FK)','fk'),
    ('+ session_date','att'),
    ('+ start_time / end_time','att'),
    ('+ status','att'),
])
entity('E_feedback','feedback',1130,600,205,[
    ('+ feedback_id (PK)','pk'),
    ('+ session_id (FK)','fk'),
    ('+ learner_id (FK)','fk'),
    ('+ teacher_id (FK)','fk'),
    ('+ performance_rating','att'),
    ('+ behavioral_notes','att'),
    ('+ is_on_time','att'),
    ('+ submitted_time','att'),
])

# Row 4 (standalone)
entity('E_settings','settings',30,870,200,[
    ('+ setting_id (PK)','pk'),
    ('+ setting_name','att'),
    ('+ setting_type','att'),
    ('+ setting_value','att'),
    ('+ priority','att'),
    ('+ is_active','att'),
])

# ── RELATIONSHIPS (Crow-foot notation) ────────────────────────────────
# roles (1) → users (N)
edge('r01','E_roles','E_users','has','ERmandOne','ERmany')
# users (1) → activity_log (N)
edge('r02','E_users','E_activity','logs','ERmandOne','ERmany')
# users (Teacher,1) → sessions (N)
edge('r03','E_users','E_sessions','teaches','ERmandOne','ERmany')
# subjects (1) → learning_packages (N)
edge('r04','E_subjects','E_packages','defines','ERmandOne','ERmany')
# subjects (1) → sessions (N)
edge('r05','E_subjects','E_sessions','schedules','ERmandOne','ERmany')
# classrooms (1) → sessions (N)
edge('r06','E_classrooms','E_sessions','hosts','ERmandOne','ERmany')
# parents (1) → learners (N)
edge('r07','E_parents','E_learners','sponsors','ERmandOne','ERmany')
# parents (1) → notifications (N)
edge('r08','E_parents','E_notif','receives','ERmandOne','ERmany')
# learners (1) → learner_packages (N)
edge('r09','E_learners','E_lpkg','enrolled in','ERmandOne','ERmany')
# learning_packages (1) → learner_packages (N)
edge('r10','E_packages','E_lpkg','assigned via','ERmandOne','ERmany')
# learners (1) → session_learners (N)
edge('r11','E_learners','E_sl','joins','ERmandOne','ERmany')
# sessions (1) → session_learners (N)
edge('r12','E_sessions','E_sl','contains','ERmandOne','ERmany')
# sessions (1) → attendance (N)
edge('r13','E_sessions','E_attend','recorded in','ERmandOne','ERmany')
# learners (1) → attendance (N)
edge('r14','E_learners','E_attend','marked in','ERmandOne','ERmany')
# sessions (1) → feedback (N)
edge('r15','E_sessions','E_feedback','generates','ERmandOne','ERmany')
# learners (1) → feedback (N)
edge('r16','E_learners','E_feedback','evaluated by','ERmandOne','ERmany')
# learners (1) → notifications (N)
edge('r17','E_learners','E_notif','notified via','ERmandOne','ERmany')
# users (Teacher) → feedback (N)
edge('r18','E_users','E_feedback','submits','ERmandOne','ERmany')

# ── BUILD FILE ────────────────────────────────────────────────────────
body = '\n'.join(cells)
xml = ('<?xml version="1.0" encoding="UTF-8"?>\n'
       '<mxfile host="app.diagrams.net" version="21.0.0">\n'
       '<diagram id="erd1" name="1.5 CLS Entity Relationship Diagram">\n'
       '<mxGraphModel dx="1422" dy="762" grid="1" gridSize="10" guides="1" tooltips="1" '
       'connect="1" arrows="1" fold="1" page="1" pageScale="1" '
       'pageWidth="1654" pageHeight="1169" math="0" shadow="0">\n'
       '<root>\n<mxCell id="0"/>\n<mxCell id="1" parent="0"/>\n'
       + body +
       '\n</root>\n</mxGraphModel>\n</diagram>\n</mxfile>')

with open(OUT,'w',encoding='utf-8') as f: f.write(xml)
print('Done. Size:', len(xml), 'bytes')
print('File:', OUT)
