const stacks = document.getElementsByClassName("stackpane");

window.addEventListener('resize', resizeStacks);

resizeStacks();

function resizeStacks(){
    for (let i = 0; i < stacks.length; i++) {
        const stack = stacks[i];
        const children = stack.children;
        let maxHeight = 0, childHeight = 0;

        for (let j = 0; j < children.length; j++) {
            const child = children[j];
            childHeight = child.offsetHeight;

            if(childHeight >= maxHeight)
                maxHeight = childHeight;
        }

        stack.style.height = maxHeight + 'px';
    }
}