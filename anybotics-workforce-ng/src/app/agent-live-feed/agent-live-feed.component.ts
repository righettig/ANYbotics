import {
  Component,
  ElementRef,
  ViewChild,
  AfterViewInit,
  HostListener,
} from '@angular/core';

import {
  Engine,
  Scene,
  ArcRotateCamera,
  Vector3,
  HemisphericLight,
  MeshBuilder,
  FreeCamera,
} from '@babylonjs/core';
import { StandardMaterial } from '@babylonjs/core/Materials/standardMaterial';
import { GridMaterial } from '@babylonjs/materials/grid/gridMaterial';
import { Color3 } from '@babylonjs/core/Maths/math.color';

@Component({
  selector: 'app-agent-live-feed',
  standalone: true,
  imports: [],
  templateUrl: './agent-live-feed.component.html',
  styleUrl: './agent-live-feed.component.scss',
})
export class AgentLiveFeedComponent implements AfterViewInit {
  @ViewChild('renderCanvas', { static: true })
  renderCanvas!: ElementRef<HTMLCanvasElement>;

  private engine!: Engine;
  private scene!: Scene;

  private topDownCamera!: ArcRotateCamera;
  private freeCamera!: FreeCamera;

  private isTopDownView = false; // Track the current view

  ngAfterViewInit(): void {
    const canvas = this.renderCanvas.nativeElement;

    this.engine = new Engine(canvas, true);
    this.scene = new Scene(this.engine);

    this.initializeScene(canvas);

    this.engine.runRenderLoop(() => {
      this.scene.render();
    });

    window.addEventListener('resize', () => {
      this.engine.resize();
    });

    // TODO: this needs to be invoked after clicking on the tab to ensure scene is correctly rendered
    this.engine.resize(); // Force engine to resize to refresh the view
  }

  private initializeScene(canvas: HTMLCanvasElement): void {
    this.createCameras(canvas);
    this.createLight();
    this.createGround();
    this.createRooms();
    this.createAnymalAgent(new Vector3(0, 0.65, 0));
  }

  private createCameras(canvas: HTMLCanvasElement): void {
    // The ArcRotateCamera is designed to orbit around a target.
    // It is ideal for situations where you want to focus on a particular object or point in the scene
    // and allow the user to rotate around it.
    // The camera orbits around a target point using three parameters: alpha, beta, and radius.
    this.topDownCamera = new ArcRotateCamera(
      'topDownCamera',
      -Math.PI / 2,
      Math.PI / 2.5,
      50,
      Vector3.Zero(),
      this.scene,
      true
    );
    this.topDownCamera.attachControl(canvas, true);

    // The FreeCamera offers more traditional first-person or free movement controls.
    // It can move freely in any direction (forward, backward, left, right, up, down) without being tied
    // to a specific target or point of interest.
    this.freeCamera = new FreeCamera(
      'freeCamera',
      new Vector3(0, 10, -30),
      this.scene,
      true
    );
    this.freeCamera.setTarget(Vector3.Zero());
    this.freeCamera.attachControl(canvas, true);

    // Initialize with the 3D camera
    this.scene.activeCamera = this.freeCamera;
  }

  private createLight(): void {
    // Add a light to the scene
    const light = new HemisphericLight(
      'light1',
      new Vector3(0, 1, 0),
      this.scene
    );
  }

  private createGround(): void {
    const gridMaterial = new GridMaterial('grid', this.scene);
    gridMaterial.gridRatio = 1;
    gridMaterial.majorUnitFrequency = 1;
    gridMaterial.minorUnitVisibility = 0.45;
    gridMaterial.backFaceCulling = false;
    gridMaterial.mainColor = new Color3(1, 1, 1);
    gridMaterial.lineColor = new Color3(0.5, 0.5, 0.5);

    // Create a ground plane
    const ground = MeshBuilder.CreateGround(
      'ground',
      { width: 50, height: 50, subdivisions: 50 },
      this.scene
    );
    ground.material = gridMaterial;
  }

  private createRooms(): void {
    const wallHeight = 3;
    const wallThickness = 0.2;
    const roomSize = 10;

    this.createRoom(
      new Vector3(-10, wallHeight / 2, -10),
      roomSize,
      wallHeight,
      wallThickness,
      new Color3(1, 0, 0) // Red object
    );
    this.createRoom(
      new Vector3(10, wallHeight / 2, -10),
      roomSize,
      wallHeight,
      wallThickness,
      new Color3(0, 1, 0) // Green object
    );
    this.createRoom(
      new Vector3(-10, wallHeight / 2, 10),
      roomSize,
      wallHeight,
      wallThickness,
      new Color3(0, 0, 1) // Blue object
    );
  }

  private createRoom(
    position: Vector3,
    size: number,
    height: number,
    thickness: number,
    objectColor: Color3
  ): void {
    const halfSize = size / 2;

    // Create walls around the room
    const wall1 = MeshBuilder.CreateBox(
      'wall1',
      { width: size, height: height, depth: thickness },
      this.scene
    );
    wall1.position = new Vector3(position.x, position.y, position.z - halfSize);

    const wall2 = MeshBuilder.CreateBox(
      'wall2',
      { width: size, height: height, depth: thickness },
      this.scene
    );
    wall2.position = new Vector3(position.x, position.y, position.z + halfSize);

    const wall3 = MeshBuilder.CreateBox(
      'wall3',
      { width: thickness, height: height, depth: size },
      this.scene
    );
    wall3.position = new Vector3(position.x - halfSize, position.y, position.z);

    const wall4 = MeshBuilder.CreateBox(
      'wall4',
      { width: thickness, height: height, depth: size },
      this.scene
    );
    wall4.position = new Vector3(position.x + halfSize, position.y, position.z);

    // Create a unique object in the room
    this.createRoomObject(position, objectColor);
  }

  private createRoomObject(position: Vector3, color: Color3): void {
    const object = MeshBuilder.CreateBox('roomObject', { size: 1 }, this.scene);
    object.position = new Vector3(position.x, 0.5, position.z); // Placed directly on the ground

    const material = new StandardMaterial('objectMaterial', this.scene);
    material.diffuseColor = color;
    object.material = material;
  }

  private createAnymalAgent(position: Vector3): void {
    // Body
    const body = MeshBuilder.CreateBox(
      'body',
      { width: 1, height: 0.5, depth: 2 },
      this.scene
    );
    body.position = new Vector3(position.x, position.y + 0.25, position.z);

    // Head
    const head = MeshBuilder.CreateBox(
      'head',
      { width: 0.6, height: 0.6, depth: 0.6 },
      this.scene
    );
    head.position = new Vector3(
      position.x,
      position.y + 0.65,
      position.z - 1.3
    );

    // Ears
    const ear1 = MeshBuilder.CreateBox(
      'ear1',
      { width: 0.2, height: 0.4, depth: 0.1 },
      this.scene
    );
    ear1.position = new Vector3(
      position.x - 0.25,
      position.y + 1,
      position.z - 1.5
    );

    const ear2 = MeshBuilder.CreateBox(
      'ear2',
      { width: 0.2, height: 0.4, depth: 0.1 },
      this.scene
    );
    ear2.position = new Vector3(
      position.x + 0.25,
      position.y + 1,
      position.z - 1.5
    );

    // Legs
    const leg1 = MeshBuilder.CreateCylinder(
      'leg1',
      { height: 0.6, diameter: 0.2 },
      this.scene
    );
    leg1.position = new Vector3(
      position.x - 0.4,
      position.y - 0.3,
      position.z + 0.7
    );

    const leg2 = MeshBuilder.CreateCylinder(
      'leg2',
      { height: 0.6, diameter: 0.2 },
      this.scene
    );
    leg2.position = new Vector3(
      position.x + 0.4,
      position.y - 0.3,
      position.z + 0.7
    );

    const leg3 = MeshBuilder.CreateCylinder(
      'leg3',
      { height: 0.6, diameter: 0.2 },
      this.scene
    );
    leg3.position = new Vector3(
      position.x - 0.4,
      position.y - 0.3,
      position.z - 0.7
    );

    const leg4 = MeshBuilder.CreateCylinder(
      'leg4',
      { height: 0.6, diameter: 0.2 },
      this.scene
    );
    leg4.position = new Vector3(
      position.x + 0.4,
      position.y - 0.3,
      position.z - 0.7
    );

    // Tail
    const tail = MeshBuilder.CreateCylinder(
      'tail',
      { height: 0.8, diameter: 0.1 },
      this.scene
    );
    tail.position = new Vector3(position.x, position.y + 0.2, position.z + 1.2);
    tail.rotation.x = Math.PI / 4; // Rotate tail upwards

    // Add some basic material to the dog for better visualization
    const dogMaterial = new StandardMaterial('dogMaterial', this.scene);
    dogMaterial.diffuseColor = new Color3(0.8, 0.5, 0.3); // Brownish color
    body.material = dogMaterial;
    head.material = dogMaterial;
    ear1.material = dogMaterial;
    ear2.material = dogMaterial;
    leg1.material = dogMaterial;
    leg2.material = dogMaterial;
    leg3.material = dogMaterial;
    leg4.material = dogMaterial;
    tail.material = dogMaterial;
  }

  toggleView(): void {
    if (this.isTopDownView) {
      this.scene.activeCamera = this.freeCamera;
    } else {
      this.scene.activeCamera = this.topDownCamera;
    }
    this.isTopDownView = !this.isTopDownView;
  }

  @HostListener('window:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent): void {
    if (event.key === 'v') {
      // Press 'v' to toggle view
      this.toggleView();
    }
  }
}
