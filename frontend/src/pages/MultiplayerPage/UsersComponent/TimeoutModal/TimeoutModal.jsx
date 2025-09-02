import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";

const TimeoutModal = ({ open, onClose, message }) => (
    <Dialog open={open} onOpenChange={onClose}>
        <DialogContent className="max-w-sm min-h-[150px]">
            <DialogHeader>
                <DialogTitle>{message}</DialogTitle>
            </DialogHeader>
            <DialogFooter className="flex items-end justify-end">
                <Button onClick={onClose}>Закрити</Button>
            </DialogFooter>
        </DialogContent>
    </Dialog>
);

export default TimeoutModal;