# Player Animation System (Quick Guide)
Gameplay code should ONLY use `PlayerAnimations`.
Do NOT use `AutoAnimation` or `AnimationController` directly.

## 1. Initialization (LoadContent)
_playerAnims = PlayerAnimations.Build(Content);

## 2. Input -> action
bool moving = false;

if (kb.IsKeyDown(Keys.A)) {
    faceLeft = true;
    pos.X -= speed * dt;
    _playerAnims.Run();
    moving = true;
}

if (kb.IsKeyDown(Keys.D)) {
    faceLeft = false;
    pos.X += speed * dt;
    _playerAnims.Run();
    moving = true;
}

if (!moving) {
    _playerAnims.Idle();
}

_playerAnims.Update(gameTime);

## 3. Draw
_playerAnims.Draw(spriteBatch, pos, Color.White, scale: 2f, faceLeft: faceLeft);

## 4. Rules
Always call Update(gameTime) once per frame
Use Idle() / Run() /... instead of touching animation internals
faceLeft = true means facing left (sprite flipped)
