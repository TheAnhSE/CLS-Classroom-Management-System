import xml.etree.ElementTree as ET
path = r'd:\Back up D and E\Work\Back up\NET\KI 8 _ Spring 2026\Block 5 _ SP2026\AI\Team Project\CLS-Classroom-Management-System\Document\02_Requirements\Use cases\CLS_UseCaseDiagrams.drawio'
try:
    tree = ET.parse(path)
    root = tree.getroot()
    diagrams = root.findall('diagram')
    print('Valid XML:', len(diagrams), 'pages')
    for d in diagrams:
        cells = d.findall('.//mxCell')
        print(' ', d.get('name'), '-', len(cells), 'cells')
except Exception as e:
    print('ERROR:', e)
