import xml.etree.ElementTree as ET
path = r'd:\Back up D and E\Work\Back up\NET\KI 8 _ Spring 2026\Block 5 _ SP2026\AI\Team Project\CLS-Classroom-Management-System\Document\02_Requirements\Workflows\CLS_Main_Workflows.drawio'
try:
    tree = ET.parse(path)
    root = tree.getroot()
    diagrams = root.findall('diagram')
    print('Valid XML:', len(diagrams), 'diagram pages')
    for d in diagrams:
        print('  Page:', d.get('name'))
    cells = root.findall('.//mxCell')
    print('Total cells:', len(cells))
except Exception as e:
    print('ERROR:', e)
