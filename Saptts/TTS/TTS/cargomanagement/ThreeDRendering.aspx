<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThreeDRendering.aspx.cs"
    Inherits="TTS.cargomanagement.ThreeDRendering" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: white;
            text-align: center;
        }
    </style>
</head>
<body>
    <form action="ThreeDRendering.aspx">
    <div>
    </div>
    </form>
    <script type="text/javascript" src="../Scripts/three.js"></script>
    <script type="text/javascript" src="../Scripts/OrbitControls.js"></script>
    <script type="text/javascript">
        var tyres_jsonObj = localStorage.getItem("tyres_jsonObj");
        tyres_jsonObj = JSON.parse(tyres_jsonObj);
        var container_jsonObj = localStorage.getItem("container_jsonObj");
        container_jsonObj = JSON.parse(container_jsonObj);
        var container;
        var camera, scene, raycaster, renderer, control, mouse = { x: 0, y: 0 }, INTERSECTED;
        var control2;
        init();
        animate();

        function init() {
            var SCREEN_WIDTH = window.innerWidth, SCREEN_HEIGHT = window.innerHeight;
            var division = document.createElement('div');
            document.body.appendChild(division);
            scene = new THREE.Scene();

            var VIEW_ANGLE = 45, ASPECT = SCREEN_WIDTH / SCREEN_HEIGHT, NEAR = 0.1, FAR = 25000;
            camera = new THREE.PerspectiveCamera(VIEW_ANGLE, ASPECT, NEAR, FAR);
            scene.add(camera);
            camera.position.set(0, 150, 6000);
            camera.lookAt(scene.position);

            renderer = new THREE.WebGLRenderer();
            renderer.setSize(SCREEN_WIDTH - 30, SCREEN_HEIGHT - 20);
            division.appendChild(renderer.domElement);

            control = new THREE.OrbitControls(camera, renderer.domElement);

            var light = new THREE.HemisphereLight(0xffffbb, 0x080820, 2)
            scene.add(light);

            var geometry = new THREE.BoxGeometry(container_jsonObj.Width, container_jsonObj.Height, container_jsonObj.Length);
            var material = new THREE.MeshBasicMaterial({ color: 0xbbbbbb, wireframe: true });
            container = new THREE.Mesh(geometry, material);
            scene.add(container)

            var conWidth = container_jsonObj.Width / 2;
            var conHeight = container_jsonObj.Height / 2;
            var condepth = container_jsonObj.Length / 2;
            Object.keys(tyres_jsonObj).forEach(function (ele) {
                var obj_geom;
                if (tyres_jsonObj[ele].orient == 3) {
                    obj_geom = new THREE.CylinderGeometry(tyres_jsonObj[ele].Length / 2, tyres_jsonObj[ele].Length / 2, tyres_jsonObj[ele].Height, 64, 50, false);
                }
                else if (tyres_jsonObj[ele].orient == 2) {
                    obj_geom = new THREE.CylinderGeometry(tyres_jsonObj[ele].Length / 2, tyres_jsonObj[ele].Length / 2, tyres_jsonObj[ele].Width, 64, 50, false);
                }
                else if (tyres_jsonObj[ele].orient == 1) {
                    obj_geom = new THREE.CylinderGeometry(tyres_jsonObj[ele].Height / 2, tyres_jsonObj[ele].Height / 2, tyres_jsonObj[ele].Length, 64, 50, false);
                }

                var obj_mat = new THREE.MeshPhongMaterial({
                    color: Math.random() * 0xffffff,
                    emissive: 0x072534,
                    side: THREE.DoubleSide
                })
                var object = new THREE.Mesh(obj_geom, obj_mat);
                object.position.x = -(conWidth) + (tyres_jsonObj[ele].Width / 2) + tyres_jsonObj[ele].x; // + (tyres_jsonObj[ele] / 2) ;//(tyres_jsonObj[ele].x/ 2) - conWidth;
                object.position.y = -(conHeight) + (tyres_jsonObj[ele].Height / 2) + tyres_jsonObj[ele].y; ;
                object.position.z = -(condepth) + (tyres_jsonObj[ele].Length / 2) + tyres_jsonObj[ele].z; ;
                object.Brand = tyres_jsonObj[ele].Brand;
                object.Sidewall = tyres_jsonObj[ele].Sidewall;
                object.Config = tyres_jsonObj[ele].Config;
                object.Tyresize = tyres_jsonObj[ele].Tyresize;
                object.Rimsize = tyres_jsonObj[ele].Rimsize;
                object.TyreType = tyres_jsonObj[ele].TyreType;
                object.Qty = tyres_jsonObj[ele].Quantity;
                if (tyres_jsonObj[ele].orient == 1) {
                    object.rotation.x = 0.5 * Math.PI;
                }
                else if (tyres_jsonObj[ele].orient == 2) {
                    object.rotation.z = 0.5 * Math.PI;
                }
                else if (tyres_jsonObj[ele].orient == 3) {
                    object.rotation.y = 0.5 * Math.PI;
                }

                if (tyres_jsonObj[ele].InTyre != null) {
                    var innerObj_geom = new THREE.CylinderGeometry(tyres_jsonObj[ele].InTyre.GetOuterDia / 2, tyres_jsonObj[ele].InTyre.GetOuterDia / 2, tyres_jsonObj[ele].InTyre.GetWidth * tyres_jsonObj[ele].InTyre.GetQuantity, 64, 50, false);
                    var innerObj_mat = new THREE.MeshPhongMaterial({
                        color: Math.random() * 0xffffff,
                        emissive: 0x072534,
                        side: THREE.DoubleSide
                    });
                    var innerObject = new THREE.Mesh(innerObj_geom, innerObj_mat);
                    object.add(innerObject);
                    object.material.wireframe = true;
                    object.inBrand = tyres_jsonObj[ele].InTyre.Brand;
                    object.inSidewall = tyres_jsonObj[ele].InTyre.Sidewall;
                    object.inConfig = tyres_jsonObj[ele].InTyre.Config;
                    object.inTyresize = tyres_jsonObj[ele].InTyre.Tyresize;
                    object.inRimsize = tyres_jsonObj[ele].InTyre.Rimsize;
                    object.inTyreType = tyres_jsonObj[ele].InTyre.GetTyreType;
                    object.inQty = tyres_jsonObj[ele].InTyre.GetQuantity;
                }
                container.add(object);
            });

            document.addEventListener('mousemove', onDocumentMouseMove, false);

            canvas1 = document.createElement('canvas');
            canvas1.style.position = "absolute";
            canvas1.style.top = "10px";
            canvas1.style.right = "20px";
            canvas1.style.zIndex = 100;
            canvas1.setAttribute("width", "220px");
            canvas1.setAttribute("height", "200px");
            context1 = canvas1.getContext('2d');
            context1.fillStyle = "rgba(0,0,0,0.95)";
            context1.fillText('', 0, 20);
            division.appendChild(canvas1);
            texture1 = new THREE.Texture(canvas1)
            texture1.needsUpdate = false;
        }

        function onDocumentMouseMove(event) {
            mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
            mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;
        }

        function animate() {
            requestAnimationFrame(animate);
            renderer.render(scene, camera);
            update();
        }

        function update() {
            var vector = new THREE.Vector3(mouse.x, mouse.y, 1);
            vector.unproject(camera);
            var ray = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());
            var intersects = ray.intersectObjects(container.children);

            if (intersects.length > 0) {
                if (intersects[0].object != INTERSECTED) {
                    if (INTERSECTED)
                        INTERSECTED.material.color.setHex(INTERSECTED.currentHex);
                    INTERSECTED = intersects[0].object;
                    INTERSECTED.currentHex = INTERSECTED.material.color.getHex();
                    INTERSECTED.material.color.setHex(0xffffff);

                    if (INTERSECTED.TyreType) {
                        context1.clearRect(0, 0, 300, 500);
                        message = "<svg xmlns='http://www.w3.org/2000/svg' width='220px' height='360px'><foreignObject width='100%' height='100%'> <div xmlns='http://www.w3.org/1999/xhtml' style='font-size:18px'>"
                        message += INTERSECTED.Config + "<br/>" + INTERSECTED.Tyresize + "<br/>" + INTERSECTED.Rimsize + "<br/>" + INTERSECTED.TyreType + "<br/>" + INTERSECTED.Brand + "<br/>" + INTERSECTED.Sidewall + "<br/>" + INTERSECTED.Qty + "<br/>";
                        if (INTERSECTED.inBrand != null) {
                            message += "--------------------------<br/>" + "INNER TYRE<br/><br/>";
                            message += INTERSECTED.inConfig + "<br/>" + INTERSECTED.inTyresize + "<br/>" + INTERSECTED.inRimsize + "<br/>" + INTERSECTED.inTyreType + "<br/>" + INTERSECTED.inBrand + "<br/>" + INTERSECTED.inSidewall + "<br/>" + INTERSECTED.inQty + "<br/><br/>";
                            canvas1.setAttribute("height", "360px");
                            context1.fillStyle = "rgba(0,0,0,1)";  // black border
                            context1.fillRect(0, 0, 200, 360);
                            context1.fillStyle = "rgba(255,255,255,1)"; // white filler
                            context1.fillRect(0, 2, 200, 360);
                            context1.fillStyle = "rgba(0,0,0,1)"; // text color
                        }
                        else {
                            canvas1.setAttribute("height", "200px");
                            context1.fillStyle = "rgba(0,0,0,1)";  // black border
                            context1.fillRect(0, 0, 160, 300);
                            context1.fillStyle = "rgba(255,255,255,1)"; // white filler
                            context1.fillRect(0, 2, 160, 320);
                            context1.fillStyle = "rgba(0,0,0,1)"; // text color
                        }
                        message += "</div></foreignObject></svg>"

                        drawInlineSVG(context1, message);
                        texture1.needsUpdate = true;
                    }
                    else {
                        context1.clearRect(0, 0, 200, 500);
                        texture1.needsUpdate = true;
                    }
                }
            }
            else {
                if (INTERSECTED)
                    INTERSECTED.material.color.setHex(INTERSECTED.currentHex);
                INTERSECTED = null;
                context1.clearRect(0, 0, 300, 500);
                texture1.needsUpdate = true;
            }
            control.update();
        }

        function drawInlineSVG(ctx, rawSVG) {
            var svg = new Blob([rawSVG], { type: "image/svg+xml;charset=utf-8" }),
            domURL = self.URL || self.webkitURL || self,
            url = domURL.createObjectURL(svg),
            img = new Image;
            img.onload = function () {
                ctx.drawImage(this, 0, 0);
                domURL.revokeObjectURL(url);
            };
            img.src = url;
        }

    </script>
</body>
</html>
